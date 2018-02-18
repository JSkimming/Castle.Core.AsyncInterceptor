// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.Invocations
{
    using System.Threading.Tasks;
    using Castle.DynamicProxy;

    /// <inheritdoc cref="IAsyncFunctionInvocation{TResult}" />
    internal class AsyncFunctionInvocation<TResult> : InvocationBase, IAsyncFunctionInvocation<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncFunctionInvocation{TResult}"/> class.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        public AsyncFunctionInvocation(IInvocation invocation)
            : base(invocation)
        {
        }

        /// <inheritdoc />
        public Task<TResult> Proceed()
        {
            Invocation.Proceed();
            return (Task<TResult>)Invocation.ReturnValue;
        }
    }
}
