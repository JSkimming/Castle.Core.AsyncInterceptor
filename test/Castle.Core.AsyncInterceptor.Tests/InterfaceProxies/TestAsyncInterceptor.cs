// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies;

public class TestAsyncInterceptor : IAsyncInterceptor
{
    private readonly ListLogger _log;

    public TestAsyncInterceptor(ListLogger log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public void InterceptSynchronous(IInvocation invocation)
    {
        LogInterceptStart(invocation);
        invocation.Proceed();
        LogInterceptEnd(invocation);
    }

    public void InterceptAsynchronous(IInvocation invocation)
    {
        invocation.ReturnValue = LogInterceptAsynchronous(invocation);
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        invocation.ReturnValue = LogInterceptAsynchronous<TResult>(invocation);
    }

    public void InterceptAsyncEnumerable<TResult>(IInvocation invocation)
    {
        LogInterceptStart(invocation);
        invocation.Proceed();
        var innerEnumerable = (IAsyncEnumerable<TResult>)invocation.ReturnValue;
        invocation.ReturnValue = LogInterceptAsyncEnumerable<TResult>(invocation, innerEnumerable);
    }

    private async Task LogInterceptAsynchronous(IInvocation invocation)
    {
        LogInterceptStart(invocation);
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task.ConfigureAwait(false);
        LogInterceptEnd(invocation);
    }

    private async Task<TResult> LogInterceptAsynchronous<TResult>(IInvocation invocation)
    {
        LogInterceptStart(invocation);
        invocation.Proceed();
        var task = (Task<TResult>)invocation.ReturnValue;
        TResult result = await task.ConfigureAwait(false);
        LogInterceptEnd(invocation);
        return result;
    }

    private async IAsyncEnumerable<TResult> LogInterceptAsyncEnumerable<TResult>(IInvocation invocation, IAsyncEnumerable<TResult> innerEnumerable)
    {
        await foreach (TResult result in innerEnumerable)
        {
            yield return result;
        }

        LogInterceptEnd(invocation);
    }

    private void LogInterceptStart(IInvocation invocation)
    {
        _log.Add($"{invocation.Method.Name}:InterceptStart");
    }

    private void LogInterceptEnd(IInvocation invocation)
    {
        _log.Add($"{invocation.Method.Name}:InterceptEnd");
    }
}
