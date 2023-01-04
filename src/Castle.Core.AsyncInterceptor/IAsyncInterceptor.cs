// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

/// <summary>
/// Implement this interface to intercept method invocations with DynamicProxy2.
/// </summary>
public interface IAsyncInterceptor
{
    /// <summary>
    /// Intercepts a synchronous method <paramref name="invocation"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    void InterceptSynchronous(IInvocation invocation);

    /// <summary>
    /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    void InterceptAsynchronous(IInvocation invocation);

    /// <summary>
    /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
    /// <param name="invocation">The method invocation.</param>
    void InterceptAsynchronous<TResult>(IInvocation invocation);

    /// <summary>
    /// Intercepts a method <paramref name="invocation"/> that returns an <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the returned enumerable.</typeparam>
    /// <param name="invocation">The method invocation.</param>
    void InterceptAsyncEnumerable<TResult>(IInvocation invocation);
}
