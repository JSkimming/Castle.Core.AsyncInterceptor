// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.InterfaceProxies;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    public class AsyncDeterminationInterceptorShould
    {
        [Fact]
        public void Implement_IInterceptor()
        {
            var sut = new AsyncDeterminationInterceptor(new Mock<IAsyncInterceptor>().Object);

            Assert.IsAssignableFrom<IInterceptor>(sut);
        }
    }

    public static class ProxyGen
    {
        public static readonly IProxyGenerator Generator = new ProxyGenerator();

        public static IInterfaceToProxy CreateProxy(ListLogger log, IAsyncInterceptor interceptor)
        {
            // Arrange
            var classWithInterfaceToProxy = new ClassWithInterfaceToProxy(log);

            IInterfaceToProxy proxy = Generator.CreateInterfaceProxyWithTargetInterface<IInterfaceToProxy>(
                classWithInterfaceToProxy,
                interceptor);

            return proxy;
        }
    }

    public class WhenInterceptingSynchronousVoidMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        public WhenInterceptingSynchronousVoidMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _proxy = ProxyGen.CreateProxy(_log, new TestAsyncInterceptor(_log));
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
        public void ShouldAllowInterceptionPriorToInvocation()
        {
            // Act
            _proxy.SynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:InterceptStart", _log[0]);
        }

        [Fact]
        public void ShouldAllowInterceptionAfterInvocation()
        {
            // Act
            _proxy.SynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:InterceptEnd", _log[3]);
        }
    }

    public class WhenInterceptingSynchronousResultMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        public WhenInterceptingSynchronousResultMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _proxy = ProxyGen.CreateProxy(_log, new TestAsyncInterceptor(_log));
        }

        [Fact]
        public void ShouldLog4Entries()
        {
            // Act
            Guid result = _proxy.SynchronousResultMethod();

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public void ShouldAllowInterceptionPriorToInvocation()
        {
            // Act
            _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:InterceptStart", _log[0]);
        }

        [Fact]
        public void ShouldAllowInterceptionAfterInvocation()
        {
            // Act
            _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:InterceptEnd", _log[3]);
        }
    }

    public class WhenInterceptingAsynchronousVoidMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        public WhenInterceptingAsynchronousVoidMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _proxy = ProxyGen.CreateProxy(_log, new TestAsyncInterceptor(_log));
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
        public async Task ShouldAllowInterceptionPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:InterceptStart", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowInterceptionAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:InterceptEnd", _log[3]);
        }
    }

    public class WhenInterceptingAsynchronousResultMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        public WhenInterceptingAsynchronousResultMethods(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
            _proxy = ProxyGen.CreateProxy(_log, new TestAsyncInterceptor(_log));
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
        public async Task ShouldAllowInterceptionPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:InterceptStart", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowInterceptionAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:InterceptEnd", _log[3]);
        }
    }
}
