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

        public TestAsyncInterceptor(List<string> log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void InterceptAction(IActionInvocation invocation)
        {
            LogInterceptStart(invocation);
            invocation.Proceed();
            LogInterceptEnd(invocation);
        }

        public TResult InterceptFunction<TResult>(IFunctionInvocation<TResult> invocation)
        {
            LogInterceptStart(invocation);
            TResult result = invocation.Proceed();
            LogInterceptEnd(invocation);
            return result;
        }

        public async Task InterceptAsyncAction(IAsyncActionInvocation invocation)
        {
            LogInterceptStart(invocation);
            Task task = invocation.ProceedAsync();
            await task.ConfigureAwait(false);
            LogInterceptEnd(invocation);
        }

        public async Task<TResult> InterceptAsyncFunction<TResult>(IAsyncFunctionInvocation<TResult> invocation)
        {
            LogInterceptStart(invocation);
            TResult result = await invocation.ProceedAsync().ConfigureAwait(false);
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
