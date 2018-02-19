// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;

    public class TestProcessingReturnValueAsyncInterceptor : ProcessingAsyncInterceptor<object>
    {
        private readonly ICollection<string> _log;

        public TestProcessingReturnValueAsyncInterceptor(List<string> log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        protected override object StartingInvocation(IAsyncInvocation invocation)
        {
            base.StartingInvocation(invocation);
            _log.Add($"{invocation.Method.Name}:StartingInvocation");
            return null;
        }

        protected override void CompletedInvocation(IAsyncInvocation invocation, object state)
        {
            _log.Add($"{invocation.Method.Name}:CompletedInvocation:(no return value)");
        }

        protected override void CompletedInvocation(IAsyncInvocation invocation, object state, object returnValue)
        {
            _log.Add($"{invocation.Method.Name}:CompletedInvocation:{returnValue ?? "(no return value)"}");
        }
    }
}
