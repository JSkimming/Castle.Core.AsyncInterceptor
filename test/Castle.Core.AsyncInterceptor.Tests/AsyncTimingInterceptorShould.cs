// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.InterfaceProxies;
    using Xunit;

    public class WhenTimingSynchronousVoidMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
        private readonly List<string> _log = new List<string>();
        private readonly TestAsyncTimingInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenTimingSynchronousVoidMethods()
        {
            _proxy = ProxyGen.CreateProxy(_log, out _interceptor);
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
        private readonly List<string> _log = new List<string>();
        private readonly TestAsyncTimingInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenTimingSynchronousResultMethods()
        {
            _proxy = ProxyGen.CreateProxy(_log, out _interceptor);
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
        private readonly List<string> _log = new List<string>();
        private readonly TestAsyncTimingInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenTimingAsynchronousVoidMethods()
        {
            _proxy = ProxyGen.CreateProxy(_log, out _interceptor);
        }

        [Fact]
        public async Task ShouldLog4Entries()
        {
            // Act
            await _proxy.AsynchronousVoidMethod();

            // Assert
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public async Task ShouldAllowTimingPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:StartingTiming", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowTimingAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedTiming:{_interceptor.Stopwatch.Elapsed:g}", _log[3]);
        }
    }

    public class WhenTimingAsynchronousResultMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
        private readonly List<string> _log = new List<string>();
        private readonly TestAsyncTimingInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenTimingAsynchronousResultMethods()
        {
            _proxy = ProxyGen.CreateProxy(_log, out _interceptor);
        }

        [Fact]
        public async Task ShouldLog4Entries()
        {
            // Act
            Guid result = await _proxy.AsynchronousResultMethod();

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public async Task ShouldAllowTimingPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:StartingTiming", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowTimingAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedTiming:{_interceptor.Stopwatch.Elapsed:g}", _log[3]);
        }
    }
}
