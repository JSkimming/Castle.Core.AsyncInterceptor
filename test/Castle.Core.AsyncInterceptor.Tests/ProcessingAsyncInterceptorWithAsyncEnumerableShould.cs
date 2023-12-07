// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
#if NET5_0_OR_GREATER

namespace Castle.DynamicProxy;

using System;
using Castle.DynamicProxy.InterfaceProxies;
using Xunit;
using Xunit.Abstractions;

public class WhenProcessingAsynchronousEnumerableMethods
{
    private const string MethodName = nameof(IInterfaceToProxy.AsynchronousEnumerableMethod);
    private readonly ListLogger _log;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingAsynchronousEnumerableMethods(ITestOutputHelper output)
    {
        _log = new ListLogger(output);
        var interceptor = new TestProcessingReturnValueAsyncInterceptor(_log);
        _proxy = ProxyGen.CreateProxy(_log, interceptor);
    }

    [Fact]
    public async Task ShouldLog4Entries()
    {
        // Arrange
        var messages = new List<string>();

        // Act
        await foreach (string msg in _proxy.AsynchronousEnumerableMethod().ConfigureAwait(false))
        {
            messages.Add(msg);
        }

        // Assert
        Assert.Equal(2, messages.Count);
        Assert.Equal("a", messages[0]);
        Assert.Equal("b", messages[1]);
        Assert.Equal(5, _log.Count);
    }

    [Fact]
    public async Task ShouldAllowProcessingPriorToInvocation()
    {
        // Arrange
        var messages = new List<string>();

        // Act
        await foreach (string msg in _proxy.AsynchronousEnumerableMethod().ConfigureAwait(false))
        {
            messages.Add(msg);
        }

        // Assert
        Assert.Equal($"{MethodName}:StartingInvocation", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowProcessingAfterInvocation()
    {
        // Arrange
        var messages = new List<string>();

        // Act
        await foreach (string msg in _proxy.AsynchronousEnumerableMethod().ConfigureAwait(false))
        {
            messages.Add(msg);
        }

        // Assert
        Assert.Equal($"{MethodName}:Yield b", _log[4]);
    }
}

public class WhenProcessingAsynchronousEnumerableMethodsThatThrowExceptions
{
    private const string MethodName = nameof(IInterfaceToProxy.AsynchronousEnumerableExceptionMethod);
    private readonly ListLogger _log;
    private readonly IInterfaceToProxy _proxy;

    public WhenProcessingAsynchronousEnumerableMethodsThatThrowExceptions(ITestOutputHelper output)
    {
        _log = new ListLogger(output);
        var interceptor = new TestProcessingReturnValueAsyncInterceptor(_log);
        _proxy = ProxyGen.CreateProxy(_log, interceptor);
    }

    [Fact]
    public async Task ShouldLog4Entries()
    {
        // Arrange
        var messages = new List<string>();
        InvalidOperationException? exception = null;

        // Act
        try
        {
            await foreach (string msg in _proxy.AsynchronousEnumerableExceptionMethod().ConfigureAwait(false))
            {
                messages.Add(msg);
            }
        }
        catch (InvalidOperationException ioe)
        {
            exception = ioe;
        }

        // Assert
        Assert.Single(messages);
        Assert.Equal("a", messages[0]);
        Assert.NotNull(exception);
        Assert.Equal(4, _log.Count);
    }

    [Fact]
    public async Task ShouldAllowProcessingPriorToInvocation()
    {
        // Arrange
        var messages = new List<string>();
        InvalidOperationException? exception = null;

        // Act
        try
        {
            await foreach (string msg in _proxy.AsynchronousEnumerableExceptionMethod().ConfigureAwait(false))
            {
                messages.Add(msg);
            }
        }
        catch (InvalidOperationException ioe)
        {
            exception = ioe;
        }

        // Assert
        Assert.Single(messages);
        Assert.Equal("a", messages[0]);
        Assert.NotNull(exception);
        Assert.Equal($"{MethodName}:StartingInvocation", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowProcessingAfterInvocation()
    {
        // Arrange
        var messages = new List<string>();
        InvalidOperationException? exception = null;

        // Act
        try
        {
            await foreach (string msg in _proxy.AsynchronousEnumerableExceptionMethod().ConfigureAwait(false))
            {
                messages.Add(msg);
            }
        }
        catch (InvalidOperationException ioe)
        {
            exception = ioe;
        }

        // Assert
        Assert.Single(messages);
        Assert.Equal("a", messages[0]);
        Assert.NotNull(exception);
        Assert.Equal($"{MethodName}:Yield a", _log[3]);
    }
}

#endif
