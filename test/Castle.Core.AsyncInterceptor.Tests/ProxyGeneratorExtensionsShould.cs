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
    using Xunit.Abstractions;

    public class ProxyGeneratorExtensionsShould
    {
        private static readonly IProxyGenerator Generator = new ProxyGenerator();
        private readonly ListLogger _log;

        public ProxyGeneratorExtensionsShould(ITestOutputHelper output)
        {
            _log = new ListLogger(output);
        }

        public static IEnumerable<object[]> InterfaceProxyFactories()
        {
            Func<IProxyGenerator, ListLogger, IInterfaceToProxy>[] proxyFactories =
            {
                (gen, log) => gen.CreateInterfaceProxyWithTarget<IInterfaceToProxy>(
                    new ClassWithInterfaceToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => gen.CreateInterfaceProxyWithTarget<IInterfaceToProxy>(
                    new ClassWithInterfaceToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTarget(
                    typeof(IInterfaceToProxy),
                    new ClassWithInterfaceToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTarget(
                    typeof(IInterfaceToProxy),
                    new ClassWithInterfaceToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTarget(
                    typeof(IInterfaceToProxy),
                    Array.Empty<Type>(),
                    new ClassWithInterfaceToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTarget(
                    typeof(IInterfaceToProxy),
                    Array.Empty<Type>(),
                    new ClassWithInterfaceToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTargetInterface(
                    typeof(IInterfaceToProxy),
                    new ClassWithInterfaceToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => gen.CreateInterfaceProxyWithTargetInterface<IInterfaceToProxy>(
                    new ClassWithInterfaceToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => gen.CreateInterfaceProxyWithTargetInterface<IInterfaceToProxy>(
                    new ClassWithInterfaceToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTargetInterface(
                    typeof(IInterfaceToProxy),
                    Array.Empty<Type>(),
                    new ClassWithInterfaceToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTargetInterface(
                    typeof(IInterfaceToProxy),
                    new ClassWithInterfaceToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTargetInterface(
                    typeof(IInterfaceToProxy),
                    Array.Empty<Type>(),
                    new ClassWithInterfaceToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
            };

            return proxyFactories.Select(p => new object[] { p });
        }

        [Theory]
        [MemberData(nameof(InterfaceProxyFactories))]
        public async Task ExtendInterfaceProxyGenerator(
            Func<IProxyGenerator, ListLogger, IInterfaceToProxy> proxyFactory)
        {
            // Act
            IInterfaceToProxy proxy = proxyFactory(Generator, _log);
            Guid result = await proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            const string methodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(4, _log.Count);
            Assert.Equal($"{methodName}:InterceptStart", _log[0]);
            Assert.Equal($"{methodName}:InterceptEnd", _log[3]);
        }

        public static IEnumerable<object[]> ClassProxyFactories()
        {
            Func<IProxyGenerator, ListLogger, ClassWithVirtualMethodToProxy>[] proxyFactories =
            {
                (gen, log) => gen.CreateClassProxyWithTarget(
                    new ClassWithVirtualMethodToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => gen.CreateClassProxyWithTarget(
                    new ClassWithVirtualMethodToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxyWithTarget(
                    typeof(ClassWithVirtualMethodToProxy),
                    Array.Empty<Type>(),
                    new ClassWithVirtualMethodToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxyWithTarget(
                    typeof(ClassWithVirtualMethodToProxy),
                    new ClassWithVirtualMethodToProxy(log),
                    ProxyGenerationOptions.Default,
                    new object[] { log },
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxyWithTarget(
                    typeof(ClassWithVirtualMethodToProxy),
                    new ClassWithVirtualMethodToProxy(log),
                    new object[] { log },
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxyWithTarget(
                    typeof(ClassWithVirtualMethodToProxy),
                    new ClassWithVirtualMethodToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxyWithTarget(
                    typeof(ClassWithVirtualMethodToProxy),
                    new ClassWithVirtualMethodToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxyWithTarget(
                    typeof(ClassWithVirtualMethodToProxy),
                    Array.Empty<Type>(),
                    new ClassWithVirtualMethodToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxyWithTarget(
                    typeof(ClassWithVirtualMethodToProxy),
                    Array.Empty<Type>(),
                    new ClassWithVirtualMethodToProxy(log),
                    ProxyGenerationOptions.Default,
                    new object[] { log },
                    new TestAsyncInterceptor(log)),
                (gen, log) => gen.CreateClassProxy<ClassWithVirtualMethodToProxy>(new TestAsyncInterceptor(log)),
                (gen, log) => gen.CreateClassProxy<ClassWithVirtualMethodToProxy>(
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxy(
                    typeof(ClassWithVirtualMethodToProxy),
                    Array.Empty<Type>(),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxy(
                    typeof(ClassWithVirtualMethodToProxy),
                    ProxyGenerationOptions.Default,
                    new object[] { log },
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxy(
                    typeof(ClassWithVirtualMethodToProxy),
                    new object[] { log },
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxy(
                    typeof(ClassWithVirtualMethodToProxy),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxy(
                    typeof(ClassWithVirtualMethodToProxy),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxy(
                    typeof(ClassWithVirtualMethodToProxy),
                    Array.Empty<Type>(),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxy(
                    typeof(ClassWithVirtualMethodToProxy),
                    Array.Empty<Type>(),
                    ProxyGenerationOptions.Default,
                    new object[] { log },
                    new TestAsyncInterceptor(log)),
            };

            return proxyFactories.Select(p => new object[] { p });
        }

        [Theory]
        [MemberData(nameof(ClassProxyFactories))]
        public async Task ExtendClassProxyGenerator(
            Func<IProxyGenerator, ListLogger, ClassWithVirtualMethodToProxy> proxyFactory)
        {
            // Act
            ClassWithVirtualMethodToProxy proxy = proxyFactory(Generator, _log);
            proxy.PostConstructorInitialize(_log);

            Guid result = await proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            const string methodName = nameof(ClassWithVirtualMethodToProxy.AsynchronousResultMethod);
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal($"{methodName}:InterceptStart", _log.First());
            Assert.Equal($"{methodName}:InterceptEnd", _log.Last());
        }
    }
}
