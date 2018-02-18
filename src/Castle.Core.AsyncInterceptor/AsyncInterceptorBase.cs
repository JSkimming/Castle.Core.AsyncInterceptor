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
        /// <inheritdoc cref="IAsyncInterceptor"/>
        public virtual void InterceptAction(IActionInvocation invocation)
        {
            invocation.Proceed();
        }

        /// <inheritdoc cref="IAsyncInterceptor"/>
        public virtual TResult InterceptFunction<TResult>(IFunctionInvocation<TResult> invocation)
        {
            return invocation.Proceed();
        }

        /// <inheritdoc cref="IAsyncInterceptor"/>
        public virtual Task<TResult> InterceptAsyncFunction<TResult>(IAsyncFunctionInvocation<TResult> invocation)
        {
            return invocation.Proceed();
        }

        /// <inheritdoc cref="IAsyncInterceptor"/>
        public virtual Task InterceptAsyncAction(IAsyncActionInvocation invocation)
        {
            return invocation.Proceed();
        }
    }
}
