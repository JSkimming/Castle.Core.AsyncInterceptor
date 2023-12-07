// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies;

public class ClassWithAlwaysCompletedAsync : IInterfaceToProxy
{
    private readonly ListLogger _log;

    public ClassWithAlwaysCompletedAsync(ListLogger log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public IReadOnlyList<string> Log => _log.GetLog();

    public void SynchronousVoidMethod()
    {
        throw new NotImplementedException();
    }

    public void SynchronousVoidExceptionMethod()
    {
        throw new NotImplementedException();
    }

    public Guid SynchronousResultMethod()
    {
        throw new NotImplementedException();
    }

    public Guid SynchronousResultExceptionMethod()
    {
        throw new NotImplementedException();
    }

    public Task AsynchronousVoidExceptionMethod()
    {
        throw new NotImplementedException();
    }

    public Task<Guid> AsynchronousResultExceptionMethod()
    {
        throw new NotImplementedException();
    }

    public Task AsynchronousVoidMethod()
    {
        _log.Add(nameof(AsynchronousVoidMethod) + ":Start");
        _log.Add(nameof(AsynchronousVoidMethod) + ":End");
        return Task.CompletedTask;
    }

    public Task<Guid> AsynchronousResultMethod()
    {
        _log.Add(nameof(AsynchronousResultMethod) + ":Start");
        _log.Add(nameof(AsynchronousResultMethod) + ":End");
        return Task.FromResult(Guid.NewGuid());
    }

#if NET5_0_OR_GREATER
    public async IAsyncEnumerable<string> AsynchronousEnumerableMethod()
    {
        _log.Add(nameof(AsynchronousEnumerableMethod) + ":Start");
        yield return "a";
        _log.Add(nameof(AsynchronousEnumerableMethod) + ":Yield a");
        await Task.Delay(10).ConfigureAwait(false);
        yield return "b";
        _log.Add(nameof(AsynchronousEnumerableMethod) + ":Yield b");
    }

    public async IAsyncEnumerable<string> AsynchronousEnumerableExceptionMethod()
    {
        _log.Add(nameof(AsynchronousEnumerableExceptionMethod) + ":Start");
        yield return "a";
        _log.Add(nameof(AsynchronousEnumerableExceptionMethod) + ":Yield a");
        await Task.Delay(10).ConfigureAwait(false);
        throw new InvalidOperationException(nameof(AsynchronousEnumerableExceptionMethod) + ":Exception");
    }
#endif
}
