// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.Invocations
{
    using System.Threading.Tasks;

    /// <inheritdoc cref="IAsyncActionInvocation" />
    internal class AsyncActionInvocation : InvocationBase, IAsyncActionInvocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncActionInvocation"/> class.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        public AsyncActionInvocation(IInvocation invocation)
            : base(invocation)
        {
        }

        /// <inheritdoc />
        public Task Proceed()
        {
            Invocation.Proceed();
            return (Task)Invocation.ReturnValue;
        }
    }
}
