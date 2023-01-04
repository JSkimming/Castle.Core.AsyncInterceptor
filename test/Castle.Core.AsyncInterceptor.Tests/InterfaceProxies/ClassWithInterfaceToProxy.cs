// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies;

public class ClassWithInterfaceToProxy : IInterfaceToProxy
{
    private readonly ListLogger _log;

    public ClassWithInterfaceToProxy(ListLogger log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public IReadOnlyList<string> Log => _log.GetLog();

    public void SynchronousVoidMethod()
    {
        _log.Add(nameof(SynchronousVoidMethod) + ":Start");
        Thread.Sleep(10);
        _log.Add(nameof(SynchronousVoidMethod) + ":End");
    }

    public void SynchronousVoidExceptionMethod()
    {
        _log.Add(nameof(SynchronousVoidExceptionMethod) + ":Start");
        Thread.Sleep(10);
        throw new InvalidOperationException(nameof(SynchronousVoidExceptionMethod) + ":Exception");
    }

    public Guid SynchronousResultMethod()
    {
        _log.Add(nameof(SynchronousResultMethod) + ":Start");
        Thread.Sleep(10);
        _log.Add(nameof(SynchronousResultMethod) + ":End");
        return Guid.NewGuid();
    }

    public Guid SynchronousResultExceptionMethod()
    {
        _log.Add(nameof(SynchronousResultExceptionMethod) + ":Start");
        Thread.Sleep(10);
        throw new InvalidOperationException(nameof(SynchronousResultExceptionMethod) + ":Exception");
    }

    public Task AsynchronousVoidMethod()
    {
        _log.Add(nameof(AsynchronousVoidMethod) + ":Start");
        return Task.Delay(10).ContinueWith(
            t => _log.Add(nameof(AsynchronousVoidMethod) + ":End"),
            TaskScheduler.Default);
    }

    public Task AsynchronousVoidExceptionMethod()
    {
        _log.Add(nameof(AsynchronousVoidExceptionMethod) + ":Start");
        return Task.Delay(10).ContinueWith(
            t => throw new InvalidOperationException(nameof(AsynchronousVoidExceptionMethod) + ":Exception"),
            TaskScheduler.Default);
    }

    public async Task<Guid> AsynchronousResultMethod()
    {
        _log.Add(nameof(AsynchronousResultMethod) + ":Start");
        await Task.Delay(10).ConfigureAwait(false);
        _log.Add(nameof(AsynchronousResultMethod) + ":End");
        return Guid.NewGuid();
    }

    public async Task<Guid> AsynchronousResultExceptionMethod()
    {
        _log.Add(nameof(AsynchronousResultExceptionMethod) + ":Start");
        await Task.Delay(10).ConfigureAwait(false);
        throw new InvalidOperationException(nameof(AsynchronousResultExceptionMethod) + ":Exception");
    }

    public async IAsyncEnumerable<Guid> AsyncEnumerableMethod()
    {
        _log.Add(nameof(AsyncEnumerableMethod) + ":Start");
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(10).ConfigureAwait(false);
            yield return Guid.NewGuid();
        }

        _log.Add(nameof(AsyncEnumerableMethod) + ":End");
    }

    public IAsyncEnumerator<Guid> AsyncEnumerableExceptionMethodNoReturnValues()
    {
        _log.Add(nameof(AsyncEnumerableExceptionMethodNoReturnValues) + ":Start");
        throw new InvalidOperationException(nameof(AsyncEnumerableExceptionMethodNoReturnValues) + ":Exception");
    }

    public async IAsyncEnumerator<Guid> AsyncEnumerableExceptionMethodReturnSomeValues()
    {
        _log.Add(nameof(AsyncEnumerableExceptionMethodReturnSomeValues) + ":Start");
        for (int i = 0; i < 2; i++)
        {
            await Task.Delay(10).ConfigureAwait(false);
            yield return Guid.NewGuid();
        }

        throw new InvalidOperationException(nameof(AsyncEnumerableExceptionMethodReturnSomeValues) + ":Exception");
    }
}
