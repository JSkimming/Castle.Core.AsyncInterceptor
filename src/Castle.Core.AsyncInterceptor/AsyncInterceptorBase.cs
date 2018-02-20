// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A base type for an <see cref="IAsyncInterceptor"/> to provided a simplified solution of method
    /// </summary>
    public abstract class AsyncInterceptorBase : IAsyncInterceptor
    {
#if !NETSTANDARD2_0
        /// <summary>
        /// A completed <see cref="Task"/>.
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(0);
#endif

        /// <inheritdoc cref="IAsyncInterceptor"/>
        void IAsyncInterceptor.InterceptAction(IActionInvocation invocation)
        {
            Task.Run(
                () => InterceptAsync(
                    invocation,
                    () =>
                    {
                        invocation.Proceed();
#if NETSTANDARD2_0
                        return Task.CompletedTask;
#else
                        return CompletedTask;
#endif
                    })).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="IAsyncInterceptor"/>
        TResult IAsyncInterceptor.InterceptFunction<TResult>(IFunctionInvocation<TResult> invocation)
        {
            return Task.Run(
                () => InterceptAsync(
                    invocation,
                    () => Task.FromResult(invocation.Proceed()))).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="IAsyncInterceptor"/>
        Task<TResult> IAsyncInterceptor.InterceptAsyncFunction<TResult>(IAsyncFunctionInvocation<TResult> invocation)
        {
            return InterceptAsync(invocation, () => invocation.ProceedAsync());
        }

        /// <inheritdoc cref="IAsyncInterceptor"/>
        Task IAsyncInterceptor.InterceptAsyncAction(IAsyncActionInvocation invocation)
        {
            return InterceptAsync(invocation, () => invocation.ProceedAsync());
        }

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceed">The function to proceed the <paramref name="invocation"/>.</param>
        /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
        protected abstract Task InterceptAsync(IAsyncInvocation invocation, Func<Task> proceed);

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceed">The function to proceed the <paramref name="invocation"/>.</param>
        /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
        protected abstract Task<TResult> InterceptAsync<TResult>(
            IAsyncInvocation invocation,
            Func<Task<TResult>> proceed);
    }
}
