// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

using System.Globalization;
using Castle.DynamicProxy.InterfaceProxies;
using Xunit;
using Xunit.Abstractions;

public class WhenProcessingSynchronousVoidMethods
{
    private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
    private readonly ListLogger _log;
    private readonly TestProcessingAsyncInterceptor _interceptor;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingSynchronousVoidMethods(ITestOutputHelper output)
    {
        string randomValue = "randomValue_" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        _log = new ListLogger(output);
        _interceptor = new TestProcessingAsyncInterceptor(_log, randomValue);
        _proxy = ProxyGen.CreateProxy(_log, _interceptor);
    }

    [Fact]
    public void ShouldLog4Entries()
    {
        // Act
        _proxy.SynchronousVoidMethod();

        // Assert
        Assert.Equal(4, _log.Count);
    }

    [Fact]
    public void ShouldAllowProcessingPriorToInvocation()
    {
        // Act
        _proxy.SynchronousVoidMethod();

        // Assert
        Assert.Equal($"{MethodName}:StartingInvocation:{_interceptor.RandomValue}", _log[0]);
    }

    [Fact]
    public void ShouldAllowProcessingAfterInvocation()
    {
        // Act
        _proxy.SynchronousVoidMethod();

        // Assert
        Assert.Equal($"{MethodName}:CompletedInvocation:{_interceptor.RandomValue}", _log[3]);
    }
}

public class WhenProcessingSynchronousResultMethods
{
    private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultMethod);
    private readonly ListLogger _log;
    private readonly TestProcessingAsyncInterceptor _interceptor;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingSynchronousResultMethods(ITestOutputHelper output)
    {
        string randomValue = "randomValue_" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        _log = new ListLogger(output);
        _interceptor = new TestProcessingAsyncInterceptor(_log, randomValue);
        _proxy = ProxyGen.CreateProxy(_log, _interceptor);
    }

    [Fact]
    public void ShouldLog4Entries()
    {
        // Act
        _proxy.SynchronousResultMethod();

        // Assert
        Assert.Equal(4, _log.Count);
    }

    [Fact]
    public void ShouldAllowProcessingPriorToInvocation()
    {
        // Act
        _proxy.SynchronousResultMethod();

        // Assert
        Assert.Equal($"{MethodName}:StartingInvocation:{_interceptor.RandomValue}", _log[0]);
    }

    [Fact]
    public void ShouldAllowProcessingAfterInvocation()
    {
        // Act
        _proxy.SynchronousResultMethod();

        // Assert
        Assert.Equal($"{MethodName}:CompletedInvocation:{_interceptor.RandomValue}", _log[3]);
    }
}

public class WhenProcessingAsynchronousVoidMethods
{
    private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
    private readonly ListLogger _log;
    private readonly TestProcessingAsyncInterceptor _interceptor;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingAsynchronousVoidMethods(ITestOutputHelper output)
    {
        string randomValue = "randomValue_" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        _log = new ListLogger(output);
        _interceptor = new TestProcessingAsyncInterceptor(_log, randomValue);
        _proxy = ProxyGen.CreateProxy(_log, _interceptor);
    }

    [Fact]
    public async Task ShouldLog4Entries()
    {
        // Act
        await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal(4, _log.Count);
    }

    [Fact]
    public async Task ShouldAllowProcessingPriorToInvocation()
    {
        // Act
        await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:StartingInvocation:{_interceptor.RandomValue}", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowProcessingAfterInvocation()
    {
        // Act
        await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:CompletedInvocation:{_interceptor.RandomValue}", _log[3]);
    }
}

public class WhenProcessingAsynchronousResultMethods
{
    private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
    private readonly ListLogger _log;
    private readonly TestProcessingAsyncInterceptor _interceptor;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingAsynchronousResultMethods(ITestOutputHelper output)
    {
        string randomValue = "randomValue_" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        _log = new ListLogger(output);
        _interceptor = new TestProcessingAsyncInterceptor(_log, randomValue);
        _proxy = ProxyGen.CreateProxy(_log, _interceptor);
    }

    [Fact]
    public async Task ShouldLog4Entries()
    {
        // Act
        Guid result = await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        Assert.Equal(4, _log.Count);
    }

    [Fact]
    public async Task ShouldAllowProcessingPriorToInvocation()
    {
        // Act
        await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:StartingInvocation:{_interceptor.RandomValue}", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowProcessingAfterInvocation()
    {
        // Act
        await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:CompletedInvocation:{_interceptor.RandomValue}", _log[3]);
    }
}

public class WhenProcessingAsyncEnumerableMethods
{
    private const string MethodName = nameof(IInterfaceToProxy.AsyncEnumerableMethod);
    private readonly ListLogger _log;
    private readonly TestProcessingAsyncInterceptor _interceptor;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingAsyncEnumerableMethods(ITestOutputHelper output)
    {
        string randomValue = "randomValue_" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        _log = new ListLogger(output);
        _interceptor = new TestProcessingAsyncInterceptor(_log, randomValue);
        _proxy = ProxyGen.CreateProxy(_log, _interceptor);
    }

    [Fact]
    public async Task ShouldLog4Entries()
    {
        // Act
        List<Guid> results = new();
        await foreach (Guid result in _proxy.AsyncEnumerableMethod())
        {
            results.Add(result);
        }

        // Assert
        Assert.Equal(10, results.Count);
        Assert.Equal(4, _log.Count);
    }

    [Fact]
    public async Task ShouldAllowProcessingPriorToInvocation()
    {
        // Act
        List<Guid> results = new();
        await foreach (Guid result in _proxy.AsyncEnumerableMethod())
        {
            results.Add(result);
        }

        // Assert
        Assert.Equal($"{MethodName}:StartingInvocation:{_interceptor.RandomValue}", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowProcessingAfterInvocation()
    {
        // Act
        List<Guid> results = new();
        await foreach (Guid result in _proxy.AsyncEnumerableMethod())
        {
            results.Add(result);
        }

        // Assert
        Assert.Equal($"{MethodName}:CompletedInvocation:{_interceptor.RandomValue}", _log[3]);
    }
}
