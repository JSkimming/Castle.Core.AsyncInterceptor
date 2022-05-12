// Copyright (c) 2016-2022 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

/// <summary>
/// An interface to be implemented, wrapped and intercepted.
/// </summary>
public interface ITarget
{
    void VoidSynchronous();

    int ResultSynchronous();

    Task CompletedTaskAsynchronous();

    Task<int> CompletedResultTaskAsynchronous();

    Task IncompleteTaskAsynchronous();

    Task<int> IncompleteResultTaskAsynchronous();
}

/// <summary>
/// The base minimal implementation of <see cref="ITarget"/>, used as the base for interception or wrapping.
/// </summary>
public class Target : ITarget
{
    private static readonly Task<int> CompletedResultTask = Task.FromResult(1);

    public void VoidSynchronous()
    {
    }

    public int ResultSynchronous() => 1;

    public Task CompletedTaskAsynchronous() => Task.CompletedTask;

    public Task<int> CompletedResultTaskAsynchronous() => CompletedResultTask;

    public async Task IncompleteTaskAsynchronous()
    {
        await Task.Yield();
    }

    public async Task<int> IncompleteResultTaskAsynchronous()
    {
        await Task.Yield();
        return 1;
    }
}

/// <summary>
/// An implementation of <see cref="ITarget"/> that does nothing more than delegate to the inner <see cref="ITarget"/>.
/// </summary>
/// <remarks>
/// This is used to represent what should be the most efficient form of interception, and is a useful baseline when
/// benchmarking.
/// </remarks>
public class TargetNopWrapper : ITarget
{
    private readonly ITarget _inner;

    public TargetNopWrapper(ITarget inner) => _inner = inner;

    public void VoidSynchronous() => _inner.VoidSynchronous();

    public int ResultSynchronous() => _inner.ResultSynchronous();

    public Task CompletedTaskAsynchronous() => _inner.CompletedTaskAsynchronous();

    public Task<int> CompletedResultTaskAsynchronous() => _inner.CompletedResultTaskAsynchronous();

    public Task IncompleteTaskAsynchronous() => _inner.IncompleteTaskAsynchronous();

    public Task<int> IncompleteResultTaskAsynchronous() => _inner.IncompleteResultTaskAsynchronous();
}

/// <summary>
/// An implementation of <see cref="ITarget"/> that introduces asynchronous operations before and after delegating to
/// the inner <see cref="ITarget"/>. Synchronous operations are pure pass-through.
/// </summary>
/// <remarks>
/// This is used to represent what should be the most efficient form of interception that includes making asynchronous
/// operations, and is a useful baseline when benchmarking.
/// </remarks>
public class TargetAsyncWrapper : ITarget
{
    private readonly ITarget _inner;

    public TargetAsyncWrapper(ITarget inner) => _inner = inner;

    public void VoidSynchronous() => _inner.VoidSynchronous();

    public int ResultSynchronous() => _inner.ResultSynchronous();

    public async Task CompletedTaskAsynchronous()
    {
        await Task.Yield();
        await _inner.CompletedTaskAsynchronous().ConfigureAwait(false);
        await Task.Yield();
    }

    public async Task<int> CompletedResultTaskAsynchronous()
    {
        await Task.Yield();
        int result = await _inner.CompletedResultTaskAsynchronous().ConfigureAwait(false);
        await Task.Yield();
        return result;
    }

    public async Task IncompleteTaskAsynchronous()
    {
        await Task.Yield();
        await _inner.IncompleteTaskAsynchronous().ConfigureAwait(false);
        await Task.Yield();
    }

    public async Task<int> IncompleteResultTaskAsynchronous()
    {
        await Task.Yield();
        int result = await _inner.IncompleteResultTaskAsynchronous().ConfigureAwait(false);
        await Task.Yield();
        return result;
    }
}
