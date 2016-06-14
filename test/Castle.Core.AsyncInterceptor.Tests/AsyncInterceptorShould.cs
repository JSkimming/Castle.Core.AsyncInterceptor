// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.InterfaceProxies;
    using Ploeh.AutoFixture;
    using Xunit;

    public class AsyncDeterminationInterceptorShould
    {
        [Fact]
        public void Implement_IInterceptor()
        {
            var fixture = new Fixture().Customize(new AsyncInterceptorCustomization());
            var sut = fixture.Create<AsyncDeterminationInterceptor>();

            Assert.IsAssignableFrom<IInterceptor>(sut);
        }
    }

    public static class ProxyGen
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        public static IInterfaceToProxy CreateProxy<TInterceptor>(List<string> log, out TInterceptor interceptor)
            where TInterceptor : class, IAsyncInterceptor
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Inject(log);
            var classWithInterfaceToProxy = fixture.Create<ClassWithInterfaceToProxy>();
            interceptor = fixture.Create<TInterceptor>();

            var proxy = Generator.CreateInterfaceProxyWithTargetInterface<IInterfaceToProxy>(
                classWithInterfaceToProxy,
                interceptor);

            return proxy;
        }

        public static IInterfaceToProxy CreateProxy<TInterceptor>(List<string> log)
            where TInterceptor : class, IAsyncInterceptor
        {
            TInterceptor unused;
            return CreateProxy(log, out unused);
        }
    }

    public class WhenInterceptingSynchronousVoidMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        public WhenInterceptingSynchronousVoidMethods()
        {
            _proxy = ProxyGen.CreateProxy<TestAsyncInterceptor>(_log);
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
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        public WhenInterceptingSynchronousResultMethods()
        {
            _proxy = ProxyGen.CreateProxy<TestAsyncInterceptor>(_log);
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
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        public WhenInterceptingAsynchronousVoidMethods()
        {
            _proxy = ProxyGen.CreateProxy<TestAsyncInterceptor>(_log);
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
        public async Task ShouldAllowInterceptionPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:InterceptStart", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowInterceptionAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:InterceptEnd", _log[3]);
        }
    }

    public class WhenInterceptingAsynchronousResultMethods
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        public WhenInterceptingAsynchronousResultMethods()
        {
            _proxy = ProxyGen.CreateProxy<TestAsyncInterceptor>(_log);
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
        public async Task ShouldAllowInterceptionPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:InterceptStart", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowInterceptionAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:InterceptEnd", _log[3]);
        }
    }
}
