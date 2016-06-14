// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class TestAsyncTimingInterceptor : AsyncTimingInterceptor
    {
        private readonly ICollection<string> _log;

        public TestAsyncTimingInterceptor(List<string> log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            _log = log;
        }

        public Stopwatch Stopwatch { get; private set; }

        protected override void StartingTiming(IInvocation invocation)
        {
            _log.Add($"{invocation.Method.Name}:StartingTiming");
        }

        protected override void CompletedTiming(IInvocation invocation, Stopwatch stopwatch)
        {
            _log.Add($"{invocation.Method.Name}:CompletedTiming:{stopwatch.Elapsed:g}");
            Stopwatch = stopwatch;
        }
    }
}
