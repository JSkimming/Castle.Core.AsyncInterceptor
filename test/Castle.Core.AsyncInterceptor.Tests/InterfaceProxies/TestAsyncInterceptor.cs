// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TestAsyncInterceptor : IAsyncInterceptor
    {
        private readonly ICollection<string> _log;

        private readonly bool _shouldProceed;

        public TestAsyncInterceptor(List<string> log, bool shouldProceed = true)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _shouldProceed = shouldProceed;
        }

        public void InterceptAction(IActionInvocation invocation)
        {
            LogInterceptStart(invocation);
            if (_shouldProceed)
                invocation.Proceed();
            LogInterceptEnd(invocation);
        }

        public TResult InterceptFunction<TResult>(IFunctionInvocation<TResult> invocation)
        {
            TResult result = default(TResult);
            LogInterceptStart(invocation);
            if (_shouldProceed)
                result = invocation.Proceed();
            LogInterceptEnd(invocation);
            return result;
        }

        public async Task InterceptAsyncAction(IAsyncActionInvocation invocation)
        {
            LogInterceptStart(invocation);
            if (_shouldProceed)
                await invocation.ProceedAsync().ConfigureAwait(false);
            LogInterceptEnd(invocation);
        }

        public async Task<TResult> InterceptAsyncFunction<TResult>(IAsyncFunctionInvocation<TResult> invocation)
        {
            TResult result = default(TResult);
            LogInterceptStart(invocation);
            if (_shouldProceed)
                result = await invocation.ProceedAsync().ConfigureAwait(false);
            LogInterceptEnd(invocation);
            return result;
        }

        private void LogInterceptStart(IAsyncInvocation invocation)
        {
            _log.Add($"{invocation.Method.Name}:InterceptStart");
        }

        private void LogInterceptEnd(IAsyncInvocation invocation)
        {
            _log.Add($"{invocation.Method.Name}:InterceptEnd");
        }
    }
}
