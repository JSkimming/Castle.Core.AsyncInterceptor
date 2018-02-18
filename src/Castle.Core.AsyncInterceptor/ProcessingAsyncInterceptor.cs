// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System.Threading.Tasks;

    /// <summary>
    /// A base type for an <see cref="IAsyncInterceptor"/> which executes only minimal processing when intercepting a
    /// method <see cref="IInvocation"/>
    /// </summary>
    /// <typeparam name="TState">
    /// The type of the custom object used to maintain state between <see cref="StartingInvocation"/> and
    /// <see cref="CompletedInvocation(IAsyncInvocation, TState, object)"/>.
    /// </typeparam>
    public abstract class ProcessingAsyncInterceptor<TState> : IAsyncInterceptor
        where TState : class
    {
        /// <inheritdoc cref="IAsyncInterceptor.InterceptAction"/>
        public void InterceptAction(IActionInvocation invocation)
        {
            TState state = StartingInvocation(invocation);
            invocation.Proceed();
            CompletedInvocation(invocation, state);
        }

        /// <inheritdoc cref="IAsyncInterceptor.InterceptFunction{TResult}"/>
        public TResult InterceptFunction<TResult>(IFunctionInvocation<TResult> invocation)
        {
            TState state = StartingInvocation(invocation);
            TResult result = invocation.Proceed();
            CompletedInvocation(invocation, state, result);
            return result;
        }

        /// <inheritdoc cref="IAsyncInterceptor.InterceptAsyncAction"/>
        public async Task InterceptAsyncAction(IAsyncActionInvocation invocation)
        {
            TState state = StartingInvocation(invocation);
            await invocation.Proceed().ConfigureAwait(false);
            CompletedInvocation(invocation, state);
        }

        /// <inheritdoc cref="IAsyncInterceptor.InterceptAsyncFunction{TResult}"/>
        public async Task<TResult> InterceptAsyncFunction<TResult>(IAsyncFunctionInvocation<TResult> invocation)
        {
            TState state = StartingInvocation(invocation);
            TResult result = await invocation.Proceed().ConfigureAwait(false);
            CompletedInvocation(invocation, state, result);
            return result;
        }

        /// <summary>
        /// Override in derived classes to receive signals prior method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <returns>The custom object used to maintain state between <see cref="StartingInvocation"/> and
        /// <see cref="CompletedInvocation(IAsyncInvocation, TState, object)"/>.</returns>
        protected virtual TState StartingInvocation(IAsyncInvocation invocation)
        {
            return null;
        }

        /// <summary>
        /// Override in derived classes to receive signals after method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">The custom object used to maintain state between
        /// <see cref="StartingInvocation(IAsyncInvocation)"/> and
        /// <see cref="CompletedInvocation(IAsyncInvocation, TState)"/>.</param>
        protected virtual void CompletedInvocation(IAsyncInvocation invocation, TState state)
        {
        }

        /// <summary>
        /// Override in derived classes to receive signals after method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="state">The custom object used to maintain state between
        /// <see cref="StartingInvocation(IAsyncInvocation)"/> and
        /// <see cref="CompletedInvocation(IAsyncInvocation, TState, object)"/>.</param>
        /// <param name="returnValue">
        /// The underlying return value of the <paramref name="invocation"/>; or <see langword="null"/> if the
        /// invocation did not return a value.
        /// </param>
        protected virtual void CompletedInvocation(IAsyncInvocation invocation, TState state, object returnValue)
        {
            CompletedInvocation(invocation, state);
        }
    }
}
