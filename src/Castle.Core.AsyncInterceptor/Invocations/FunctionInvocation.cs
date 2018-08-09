﻿// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.Invocations
{
    using System.Collections.Generic;

    /// <inheritdoc cref="IFunctionInvocation{TResult}" />
    internal class FunctionInvocation<TResult> : InvocationBase, IFunctionInvocation<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionInvocation{TResult}"/> class.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        public FunctionInvocation(IInvocation invocation)
            : base(invocation)
        {
        }

        /// <inheritdoc />
        public TResult Proceed()
        {
            Invocation.Proceed();
            return (TResult)Invocation.ReturnValue;
        }
    }
}
