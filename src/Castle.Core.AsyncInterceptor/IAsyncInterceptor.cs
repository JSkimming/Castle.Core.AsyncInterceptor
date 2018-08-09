// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System.Threading.Tasks;

    /// <summary>
    /// Implement this interface to intercept method invocations with DynamicProxy2.
    /// </summary>
    public interface IAsyncInterceptor
    {
        /// <summary>
        /// Intercepts a synchronous method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        void InterceptAction(IActionInvocation invocation);

        /// <summary>
        /// Intercepts a synchronous method <paramref name="invocation"/> with return type of T.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <typeparam name="TResult">Return type of the intercepted function</typeparam>
        /// <returns>The return value of the intercepted call</returns>
        TResult InterceptFunction<TResult>(IFunctionInvocation<TResult> invocation);

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <returns>The asynchronous task</returns>
        Task InterceptAsyncAction(IAsyncActionInvocation invocation);

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        /// <returns>The return value of the intercepted call</returns>
        Task<TResult> InterceptAsyncFunction<TResult>(IAsyncFunctionInvocation<TResult> invocation);
    }
}
