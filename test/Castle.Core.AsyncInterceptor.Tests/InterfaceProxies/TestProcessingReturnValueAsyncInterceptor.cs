// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;

    public class TestProcessingReturnValueAsyncInterceptor : ProcessingAsyncInterceptor<object>
    {
        private readonly ListLogger _log;

        public TestProcessingReturnValueAsyncInterceptor(ListLogger log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        protected override object StartingInvocation(IInvocation invocation)
        {
            _log.Add($"{invocation.Method.Name}:StartingInvocation");
            return string.Empty;
        }

        protected override void CompletedInvocation(IInvocation invocation, object state, object? returnValue)
        {
            base.CompletedInvocation(invocation, state, returnValue);
            _log.Add($"{invocation.Method.Name}:CompletedInvocation:{returnValue ?? "(no return value)"}");
        }
    }
}
