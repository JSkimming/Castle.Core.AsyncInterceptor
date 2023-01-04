// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;
using System.Collections.Generic;

/// <summary>
/// A base type for an <see cref="IAsyncInterceptor"/> which executes only minimal processing when intercepting a
/// method <see cref="IInvocation"/>.
/// </summary>
/// <typeparam name="TState">
/// The type of the custom object used to maintain state between <see cref="StartingInvocation"/> and
/// <see cref="CompletedInvocation(IInvocation, TState, object)"/>.
/// </typeparam>
public abstract class ProcessingAsyncInterceptor<TState> : IAsyncInterceptor
    where TState : class
{
    /// <summary>
    /// Intercepts a synchronous method <paramref name="invocation"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void InterceptSynchronous(IInvocation invocation)
    {
        TState state = Proceed(invocation);

        // Signal that the invocation has been completed.
        CompletedInvocation(invocation, state, invocation.ReturnValue);
    }

    /// <summary>
    /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void InterceptAsynchronous(IInvocation invocation)
    {
        TState state = Proceed(invocation);
        invocation.ReturnValue = SignalWhenCompleteAsync(invocation, state);
    }

    /// <summary>
    /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
    /// <param name="invocation">The method invocation.</param>
    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        TState state = Proceed(invocation);
        invocation.ReturnValue = SignalWhenCompleteAsync<TResult>(invocation, state);
    }

    /// <summary>
    /// Intercepts a method <paramref name="invocation"/> with return type of <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of ther returned enumerable.</typeparam>
    /// <param name="invocation">The method invocation.</param>
    public void InterceptAsyncEnumerable<TResult>(IInvocation invocation)
    {
        TState state = Proceed(invocation);
        var innerAsync = (IAsyncEnumerable<TResult>)invocation.ReturnValue;
        invocation.ReturnValue = SignalWhenEnumerationCompleteAsync<TResult>(invocation, innerAsync, state);
    }

    /// <summary>
    /// Override in derived classes to receive signals prior method <paramref name="invocation"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    /// <returns>The custom object used to maintain state between <see cref="StartingInvocation"/> and
    /// <see cref="CompletedInvocation(IInvocation, TState, object)"/>.</returns>
    protected abstract TState StartingInvocation(IInvocation invocation);

    /// <summary>
    /// Override in derived classes to receive signals after method <paramref name="invocation"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    /// <param name="state">The custom object used to maintain state between
    /// <see cref="StartingInvocation(IInvocation)"/> and
    /// <see cref="CompletedInvocation(IInvocation, TState)"/>.</param>
    protected virtual void CompletedInvocation(IInvocation invocation, TState state)
    {
    }

    /// <summary>
    /// Override in derived classes to receive signals after method <paramref name="invocation"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    /// <param name="state">The custom object used to maintain state between
    /// <see cref="StartingInvocation(IInvocation)"/> and
    /// <see cref="CompletedInvocation(IInvocation, TState, object)"/>.</param>
    /// <param name="returnValue">
    /// The underlying return value of the <paramref name="invocation"/>; or <see langword="null"/> if the
    /// invocation did not return a value.
    /// </param>
    protected virtual void CompletedInvocation(IInvocation invocation, TState state, object? returnValue)
    {
        CompletedInvocation(invocation, state);
    }

    /// <summary>
    /// Signals the <see cref="StartingInvocation"/> then <see cref="IInvocation.Proceed"/> on the
    /// <paramref name="invocation"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    /// <returns>The <typeparamref name="TState"/> returned by <see cref="StartingInvocation"/>.</returns>
    private TState Proceed(IInvocation invocation)
    {
        // Signal that the invocation is about to be started.
        TState state = StartingInvocation(invocation);

        // Execute the invocation.
        invocation.Proceed();

        return state;
    }

    /// <summary>
    /// Returns a <see cref="Task"/> that replaces the <paramref name="invocation"/>
    /// <see cref="IInvocation.ReturnValue"/>, that only completes after
    /// <see cref="CompletedInvocation(IInvocation, TState, object)"/> has been signaled.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    /// <param name="state">
    /// The <typeparamref name="TState"/> returned by <see cref="StartingInvocation"/>.
    /// </param>
    private async Task SignalWhenCompleteAsync(IInvocation invocation, TState state)
    {
        // Get the task to await.
        var returnValue = (Task)invocation.ReturnValue;

        await returnValue.ConfigureAwait(false);

        // Signal that the invocation has been completed.
        CompletedInvocation(invocation, state, null);
    }

    /// <summary>
    /// Returns a <see cref="Task{TResult}"/> that replaces the <paramref name="invocation"/>
    /// <see cref="IInvocation.ReturnValue"/>, that only completes after
    /// <see cref="CompletedInvocation(IInvocation, TState, object)"/> has been signaled.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    /// <param name="state">
    /// The <typeparamref name="TState"/> returned by <see cref="StartingInvocation"/>.
    /// </param>
    private async Task<TResult> SignalWhenCompleteAsync<TResult>(IInvocation invocation, TState state)
    {
        // Get the task to await.
        var returnValue = (Task<TResult>)invocation.ReturnValue;

        TResult result = await returnValue.ConfigureAwait(false);

        // Signal that the invocation has been completed.
        CompletedInvocation(invocation, state, result);

        return result;
    }

    private async IAsyncEnumerable<TResult> SignalWhenEnumerationCompleteAsync<TResult>(IInvocation invocation, IAsyncEnumerable<TResult> innerAsync, TState state)
    {
        // loop / yield the proxied method
        await foreach (TResult item in innerAsync)
        {
            yield return item;
        }

        // Signal that the invocation has been completed.
        CompletedInvocation(invocation, state);
    }
}
