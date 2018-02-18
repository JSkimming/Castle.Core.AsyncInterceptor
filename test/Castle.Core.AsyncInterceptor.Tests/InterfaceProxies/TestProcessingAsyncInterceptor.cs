// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;

    public class TestProcessingAsyncInterceptor : ProcessingAsyncInterceptor<string>
    {
        private readonly ICollection<string> _log;

        public TestProcessingAsyncInterceptor(List<string> log, string randomValue)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            RandomValue = randomValue ?? throw new ArgumentNullException(nameof(randomValue));
        }

        public string RandomValue { get; }

        protected override string StartingInvocation(IAsyncInvocation invocation)
        {
            base.StartingInvocation(invocation);
            _log.Add($"{invocation.Method.Name}:StartingInvocation:{RandomValue}");
            return RandomValue;
        }

        protected override void CompletedInvocation(IAsyncInvocation invocation, string state)
        {
            base.CompletedInvocation(invocation, state);
            _log.Add($"{invocation.Method.Name}:CompletedInvocation:{RandomValue}");
        }
    }
}
