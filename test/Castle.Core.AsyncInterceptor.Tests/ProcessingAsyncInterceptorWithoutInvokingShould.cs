// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

using Castle.DynamicProxy.InterfaceProxies;
using Xunit;
using Xunit.Abstractions;

public class WhenProcessingSynchronousVoidMethodsWithoutInvoking
{
    private readonly ListLogger _log;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingSynchronousVoidMethodsWithoutInvoking(ITestOutputHelper output)
    {
        _log = new ListLogger(output);
        var interceptor = new TestProcessingReturnValueWithoutInvokingAsyncInterceptor(_log);
        _proxy = ProxyGen.CreateProxy(_log, interceptor);
    }

    [Fact]
    public void ShouldLog4Entries()
    {
        // Act
        _proxy.SynchronousVoidMethod();

        // Assert
        Assert.Equal(2, _log.Count);
    }
}

public class WhenProcessingSynchronousResultMethodsWithoutInvoking
{
    private readonly ListLogger _log;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingSynchronousResultMethodsWithoutInvoking(ITestOutputHelper output)
    {
        _log = new ListLogger(output);
        var interceptor = new TestProcessingReturnValueWithoutInvokingAsyncInterceptor(_log);
        _proxy = ProxyGen.CreateProxy(_log, interceptor);
    }

    [Fact]
    public void ShouldLog4Entries()
    {
        // Act
        Guid result = _proxy.SynchronousResultMethod();

        // Assert
        Assert.Equal(Guid.Empty, result);
        Assert.Equal(2, _log.Count);
    }
}

public class WhenProcessingAsynchronousVoidMethodsWithoutInvoking
{
    private readonly ListLogger _log;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingAsynchronousVoidMethodsWithoutInvoking(ITestOutputHelper output)
    {
        _log = new ListLogger(output);
        var interceptor = new TestProcessingReturnValueWithoutInvokingAsyncInterceptor(_log);
        _proxy = ProxyGen.CreateProxy(_log, interceptor);
    }

    [Fact]
    public async Task ShouldLog4Entries()
    {
        // Act
        await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal(2, _log.Count);
    }
}

public class WhenProcessingAsynchronousResultMethodsWithoutInvoking
{
    private readonly ListLogger _log;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingAsynchronousResultMethodsWithoutInvoking(ITestOutputHelper output)
    {
        _log = new ListLogger(output);
        var interceptor = new TestProcessingReturnValueWithoutInvokingAsyncInterceptor(_log);
        _proxy = ProxyGen.CreateProxy(_log, interceptor);
    }

    [Fact]
    public async Task ShouldLog4Entries()
    {
        // Act
        Guid result = await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal(Guid.Empty, result);
        Assert.Equal(2, _log.Count);
    }
}
