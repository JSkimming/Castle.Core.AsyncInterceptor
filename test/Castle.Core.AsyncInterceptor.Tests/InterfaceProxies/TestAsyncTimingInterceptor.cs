﻿// Copyright (c) 2016 James Skimming. All rights reserved.
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
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Stopwatch Stopwatch { get; private set; }

        protected override void StartingTiming(IAsyncInvocation invocation)
        {
            _log.Add($"{invocation.Method.Name}:StartingTiming");
        }

        protected override void CompletedTiming(IAsyncInvocation invocation, Stopwatch stopwatch)
        {
            _log.Add($"{invocation.Method.Name}:CompletedTiming:{stopwatch.Elapsed:g}");
            Stopwatch = stopwatch;
        }
    }
}
