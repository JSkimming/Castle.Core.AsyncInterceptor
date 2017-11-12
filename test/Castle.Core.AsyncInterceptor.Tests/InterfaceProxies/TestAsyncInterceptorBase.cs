// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TestAsyncInterceptorBase : AsyncInterceptorBase
    {
        private readonly int _msDeley;
        private readonly ListLogger _log;

        public TestAsyncInterceptorBase(ListLogger log, int msDeley)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _msDeley = msDeley;
        }

        protected override async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            try
            {
                _log.Add($"{invocation.Method.Name}:StartingVoidInvocation");

                await Task.Yield();
                await proceed(invocation).ConfigureAwait(false);

                if (_msDeley > 0)
                    await Task.Delay(_msDeley).ConfigureAwait(false);

                _log.Add($"{invocation.Method.Name}:CompletedVoidInvocation");
            }
            catch (Exception e)
            {
                _log.Add($"{invocation.Method.Name}:VoidExceptionThrown:{e.Message}");
                throw;
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation,
            Func<IInvocation, Task<TResult>> proceed)
        {
            try
            {
                _log.Add($"{invocation.Method.Name}:StartingResultInvocation");

                TResult result = await proceed(invocation).ConfigureAwait(false);

                if (_msDeley > 0)
                    await Task.Delay(_msDeley).ConfigureAwait(false);

                _log.Add($"{invocation.Method.Name}:CompletedResultInvocation");
                return result;
            }
            catch (Exception e)
            {
                _log.Add($"{invocation.Method.Name}:ResultExceptionThrown:{e.Message}");
                throw;
            }
        }
    }
}
