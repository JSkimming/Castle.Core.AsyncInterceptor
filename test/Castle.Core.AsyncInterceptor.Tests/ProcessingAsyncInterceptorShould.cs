// Copyright (c) 2016-2020 James Skimming. All rights reserved.
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

    public class WhenProcessingSynchronousVoidMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
        private readonly ListLogger _log;
        private readonly TestProcessingAsyncInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenProcessingSynchronousVoidMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _interceptor = new TestProcessingAsyncInterceptor(_log, "randomValue_" + Guid.NewGuid().ToString("N"));
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
            _log = new ListLogger(output);
            _interceptor = new TestProcessingAsyncInterceptor(_log, "randomValue_" + Guid.NewGuid().ToString("N"));
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
            _log = new ListLogger(output);
            _interceptor = new TestProcessingAsyncInterceptor(_log, "randomValue_" + Guid.NewGuid().ToString("N"));
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
            _log = new ListLogger(output);
            _interceptor = new TestProcessingAsyncInterceptor(_log, "randomValue_" + Guid.NewGuid().ToString("N"));
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
}
