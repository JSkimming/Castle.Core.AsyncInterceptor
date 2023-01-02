// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies;

public class TestAsyncInterceptorBase : AsyncInterceptorBase
{
    private readonly ListLogger _log;
    private readonly bool _asyncB4Proceed;
    private readonly int _msDelayAfterProceed;

    public TestAsyncInterceptorBase(ListLogger log, bool asyncB4Proceed, int msDelayAfterProceed)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _asyncB4Proceed = asyncB4Proceed;
        _msDelayAfterProceed = msDelayAfterProceed;
    }

    protected override async Task InterceptAsync(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        try
        {
            _log.Add($"{invocation.Method.Name}:StartingVoidInvocation");

            if (_asyncB4Proceed)
                await Task.Yield();

            await proceed(invocation, proceedInfo).ConfigureAwait(false);

            if (_msDelayAfterProceed > 0)
                await Task.Delay(_msDelayAfterProceed).ConfigureAwait(false);

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
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        try
        {
            _log.Add($"{invocation.Method.Name}:StartingResultInvocation");

            if (_asyncB4Proceed)
                await Task.Yield();

            TResult result = await proceed(invocation, proceedInfo).ConfigureAwait(false);

            if (_msDelayAfterProceed > 0)
                await Task.Delay(_msDelayAfterProceed).ConfigureAwait(false);

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
