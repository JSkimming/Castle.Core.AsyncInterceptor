// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class TestAsyncInterceptorBase : AsyncInterceptorBase
    {
        private readonly int _msDeley;
        private readonly ICollection<string> _log;

        public TestAsyncInterceptorBase(List<string> log, int msDeley)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _msDeley = msDeley;
        }

        public override void InterceptAction(IActionInvocation invocation)
        {
            try
            {
                _log.Add($"{invocation.Method.Name}:StartingVoidInvocation");

                invocation.Proceed();

                if (_msDeley > 0)
                    Thread.Sleep(_msDeley);

                _log.Add($"{invocation.Method.Name}:CompletedVoidInvocation");
            }
            catch (Exception e)
            {
                _log.Add($"{invocation.Method.Name}:VoidExceptionThrown:{e.Message}");
                throw;
            }
        }

        public override TResult InterceptFunction<TResult>(
            IFunctionInvocation<TResult> invocation)
        {
            try
            {
                _log.Add($"{invocation.Method.Name}:StartingResultInvocation");

                TResult result = invocation.Proceed();

                if (_msDeley > 0)
                    Thread.Sleep(_msDeley);

                _log.Add($"{invocation.Method.Name}:CompletedResultInvocation");
                return result;
            }
            catch (Exception e)
            {
                _log.Add($"{invocation.Method.Name}:ResultExceptionThrown:{e.Message}");
                throw;
            }
        }

        public override async Task InterceptAsyncAction(IAsyncActionInvocation invocation)
        {
            try
            {
                _log.Add($"{invocation.Method.Name}:StartingVoidInvocation");

                await invocation.Proceed().ConfigureAwait(false);

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

        public override async Task<TResult> InterceptAsyncFunction<TResult>(
            IAsyncFunctionInvocation<TResult> invocation)
        {
            try
            {
                _log.Add($"{invocation.Method.Name}:StartingResultInvocation");

                TResult result = await invocation.Proceed().ConfigureAwait(false);

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
