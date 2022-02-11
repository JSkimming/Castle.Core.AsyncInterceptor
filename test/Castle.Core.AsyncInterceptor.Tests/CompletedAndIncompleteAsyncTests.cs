// Copyright (c) 2016-2022 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy.InterfaceProxies;
using Xunit;
using Xunit.Abstractions;

public abstract class AsynchronousVoidMethodCompletedAndIncompleteBase
{
    private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
    private readonly ListLogger _log;
    private readonly IInterfaceToProxy _proxy;

    protected AsynchronousVoidMethodCompletedAndIncompleteBase(ITestOutputHelper output, bool alwaysCompleted)
    {
        _log = new ListLogger(output);

        IInterfaceToProxy AlwaysCompletedFactory() => new ClassWithAlwaysCompletedAsync(_log);
        IInterfaceToProxy AlwaysIncompleteFactory() => new ClassWithAlwaysIncompleteAsync(_log);

        var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed: true, msDelayAfterProceed: 10);
        _proxy = ProxyGen.CreateProxy(
            alwaysCompleted ? AlwaysCompletedFactory : (Func<IInterfaceToProxy>)AlwaysIncompleteFactory,
            interceptor);
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
        Assert.Equal($"{MethodName}:StartingVoidInvocation", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowProcessingAfterInvocation()
    {
        // Act
        await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:CompletedVoidInvocation", _log[3]);
    }
}

public class WhenInterceptingAsynchronousVoidMethodsWhichReturnCompletedTasks
    : AsynchronousVoidMethodCompletedAndIncompleteBase
{
    public WhenInterceptingAsynchronousVoidMethodsWhichReturnCompletedTasks(ITestOutputHelper output)
        : base(output, alwaysCompleted: true)
    {
    }
}

public class WhenInterceptingAsynchronousVoidMethodsWhichReturnIncompleteTasks
    : AsynchronousVoidMethodCompletedAndIncompleteBase
{
    public WhenInterceptingAsynchronousVoidMethodsWhichReturnIncompleteTasks(ITestOutputHelper output)
        : base(output, alwaysCompleted: false)
    {
    }
}

public abstract class AsynchronousResultMethodCompletedAndIncompleteBase
{
    private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
    private readonly ListLogger _log;
    private readonly IInterfaceToProxy _proxy;

    protected AsynchronousResultMethodCompletedAndIncompleteBase(ITestOutputHelper output, bool alwaysCompleted)
    {
        _log = new ListLogger(output);

        IInterfaceToProxy AlwaysCompletedFactory() => new ClassWithAlwaysCompletedAsync(_log);
        IInterfaceToProxy AlwaysIncompleteFactory() => new ClassWithAlwaysIncompleteAsync(_log);

        var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed: true, msDelayAfterProceed: 10);
        _proxy = ProxyGen.CreateProxy(
            alwaysCompleted ? AlwaysCompletedFactory : (Func<IInterfaceToProxy>)AlwaysIncompleteFactory,
            interceptor);
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
        Assert.Equal($"{MethodName}:StartingResultInvocation", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowProcessingAfterInvocation()
    {
        // Act
        await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:CompletedResultInvocation", _log[3]);
    }
}

public class WhenInterceptingAsynchronousResultMethodsWhichReturnCompletedTasks
    : AsynchronousResultMethodCompletedAndIncompleteBase
{
    public WhenInterceptingAsynchronousResultMethodsWhichReturnCompletedTasks(ITestOutputHelper output)
        : base(output, alwaysCompleted: true)
    {
    }
}

public class WhenInterceptingAsynchronousResultMethodsWhichReturnIncompleteTasks
    : AsynchronousResultMethodCompletedAndIncompleteBase
{
    public WhenInterceptingAsynchronousResultMethodsWhichReturnIncompleteTasks(ITestOutputHelper output)
        : base(output, alwaysCompleted: false)
    {
    }
}

public abstract class TimingAsynchronousVoidMethodCompletedAndIncompleteBase
{
    private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
    private readonly ListLogger _log;
    private readonly TestAsyncTimingInterceptor _interceptor;
    private readonly IInterfaceToProxy _proxy;

    protected TimingAsynchronousVoidMethodCompletedAndIncompleteBase(ITestOutputHelper output, bool alwaysCompleted)
    {
        _log = new ListLogger(output);

        IInterfaceToProxy AlwaysCompletedFactory() => new ClassWithAlwaysCompletedAsync(_log);
        IInterfaceToProxy AlwaysIncompleteFactory() => new ClassWithAlwaysIncompleteAsync(_log);

        _interceptor = new TestAsyncTimingInterceptor(_log);
        _proxy = ProxyGen.CreateProxy(
            alwaysCompleted ? AlwaysCompletedFactory : (Func<IInterfaceToProxy>)AlwaysIncompleteFactory,
            _interceptor);
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
    public async Task ShouldAllowTimingPriorToInvocation()
    {
        // Act
        await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:StartingTiming", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowTimingAfterInvocation()
    {
        // Act
        await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:CompletedTiming:{_interceptor.Stopwatch.Elapsed:g}", _log[3]);
    }
}

public class WhenTimingAsynchronousVoidMethodsWhichReturnCompletedTasks
    : TimingAsynchronousVoidMethodCompletedAndIncompleteBase
{
    public WhenTimingAsynchronousVoidMethodsWhichReturnCompletedTasks(ITestOutputHelper output)
        : base(output, alwaysCompleted: true)
    {
    }
}

public class WhenTimingAsynchronousVoidMethodsWhichReturnIncompleteTasks
    : TimingAsynchronousVoidMethodCompletedAndIncompleteBase
{
    public WhenTimingAsynchronousVoidMethodsWhichReturnIncompleteTasks(ITestOutputHelper output)
        : base(output, alwaysCompleted: false)
    {
    }
}

public abstract class TimingAsynchronousResultMethodCompletedAndIncompleteBase
{
    private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
    private readonly ListLogger _log;
    private readonly TestAsyncTimingInterceptor _interceptor;
    private readonly IInterfaceToProxy _proxy;

    protected TimingAsynchronousResultMethodCompletedAndIncompleteBase(ITestOutputHelper output, bool alwaysCompleted)
    {
        _log = new ListLogger(output);

        IInterfaceToProxy AlwaysCompletedFactory() => new ClassWithAlwaysCompletedAsync(_log);
        IInterfaceToProxy AlwaysIncompleteFactory() => new ClassWithAlwaysIncompleteAsync(_log);

        _interceptor = new TestAsyncTimingInterceptor(_log);
        _proxy = ProxyGen.CreateProxy(
            alwaysCompleted ? AlwaysCompletedFactory : (Func<IInterfaceToProxy>)AlwaysIncompleteFactory,
            _interceptor);
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
    public async Task ShouldAllowTimingPriorToInvocation()
    {
        // Act
        await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:StartingTiming", _log[0]);
    }

    [Fact]
    public async Task ShouldAllowTimingAfterInvocation()
    {
        // Act
        await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

        // Assert
        Assert.Equal($"{MethodName}:CompletedTiming:{_interceptor.Stopwatch.Elapsed:g}", _log[3]);
    }
}

public class WhenTimingAsynchronousResultMethodsWhichReturnCompletedTasks
    : TimingAsynchronousResultMethodCompletedAndIncompleteBase
{
    public WhenTimingAsynchronousResultMethodsWhichReturnCompletedTasks(ITestOutputHelper output)
        : base(output, alwaysCompleted: true)
    {
    }
}

public class WhenTimingAsynchronousResultMethodsWhichReturnIncompleteTasks
    : TimingAsynchronousResultMethodCompletedAndIncompleteBase
{
    public WhenTimingAsynchronousResultMethodsWhichReturnIncompleteTasks(ITestOutputHelper output)
        : base(output, alwaysCompleted: false)
    {
    }
}
