// Copyright (c) 2016-2021 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.InterfaceProxies;
    using Xunit;
    using Xunit.Abstractions;

    public class WhenTimingSynchronousVoidMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
        private readonly ListLogger _log;
        private readonly TestAsyncTimingInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenTimingSynchronousVoidMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _interceptor = new TestAsyncTimingInterceptor(_log);
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
        public void ShouldAllowTimingPriorToInvocation()
        {
            // Act
            _proxy.SynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:StartingTiming", _log[0]);
        }

        [Fact]
        public void ShouldAllowTimingAfterInvocation()
        {
            // Act
            _proxy.SynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedTiming:{_interceptor.Stopwatch.Elapsed:g}", _log[3]);
        }
    }

    public class WhenTimingSynchronousResultMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultMethod);
        private readonly ListLogger _log;
        private readonly TestAsyncTimingInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenTimingSynchronousResultMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _interceptor = new TestAsyncTimingInterceptor(_log);
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
        public void ShouldAllowTimingPriorToInvocation()
        {
            // Act
            _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:StartingTiming", _log[0]);
        }

        [Fact]
        public void ShouldAllowTimingAfterInvocation()
        {
            // Act
            _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedTiming:{_interceptor.Stopwatch.Elapsed:g}", _log[3]);
        }
    }

    public class WhenTimingAsynchronousVoidMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
        private readonly ListLogger _log;
        private readonly TestAsyncTimingInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenTimingAsynchronousVoidMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _interceptor = new TestAsyncTimingInterceptor(_log);
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

    public class WhenTimingAsynchronousResultMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
        private readonly ListLogger _log;
        private readonly TestAsyncTimingInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenTimingAsynchronousResultMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _interceptor = new TestAsyncTimingInterceptor(_log);
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
}
