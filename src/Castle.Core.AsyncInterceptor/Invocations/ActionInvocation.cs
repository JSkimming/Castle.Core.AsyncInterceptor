// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.Invocations
{
    using Castle.DynamicProxy;

    /// <inheritdoc cref="IActionInvocation" />
    internal class ActionInvocation : InvocationBase, IActionInvocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInvocation"/> class.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        public ActionInvocation(IInvocation invocation)
            : base(invocation)
        {
        }

        /// <inheritdoc />
        public void Proceed()
        {
            Invocation.Proceed();
        }
    }
}
