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

    public class ProxyGeneratorExtensionsShould
    {
        private static readonly IProxyGenerator Generator = new ProxyGenerator();

        public static IEnumerable<object[]> InterfaceProxyFactories()
        {
            Func<IProxyGenerator, List<string>, IInterfaceToProxy>[] proxyFactories =
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
                    default(Type[]),
                    new ClassWithInterfaceToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTarget(
                    typeof(IInterfaceToProxy),
                    default(Type[]),
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
                    default(Type[]),
                    new ClassWithInterfaceToProxy(log),
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTargetInterface(
                    typeof(IInterfaceToProxy),
                    new ClassWithInterfaceToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (IInterfaceToProxy) gen.CreateInterfaceProxyWithTargetInterface(
                    typeof(IInterfaceToProxy),
                    default(Type[]),
                    new ClassWithInterfaceToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
            };

            return proxyFactories.Select(p => new object[] { p, new List<string>() });
        }

        [Theory]
        [MemberData(nameof(InterfaceProxyFactories))]
        public async Task ExtendInterfaceProxyGenerator(
            Func<IProxyGenerator, List<string>, IInterfaceToProxy> proxyFactory,
            List<string> log)
        {
            // Act
            IInterfaceToProxy proxy = proxyFactory(Generator, log);
            Guid result = await proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            const string methodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(4, log.Count);
            Assert.Equal($"{methodName}:InterceptStart", log[0]);
            Assert.Equal($"{methodName}:InterceptEnd", log[3]);
        }

        public static IEnumerable<object[]> ClassProxyFactories()
        {
            Func<IProxyGenerator, List<string>, ClassWithVirtualMethodToProxy>[] proxyFactories =
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
                    default(Type[]),
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
                    default(Type[]),
                    new ClassWithVirtualMethodToProxy(log),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxyWithTarget(
                    typeof(ClassWithVirtualMethodToProxy),
                    default(Type[]),
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
                    default(Type[]),
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
                    default(Type[]),
                    ProxyGenerationOptions.Default,
                    new TestAsyncInterceptor(log)),
                (gen, log) => (ClassWithVirtualMethodToProxy) gen.CreateClassProxy(
                    typeof(ClassWithVirtualMethodToProxy),
                    default(Type[]),
                    ProxyGenerationOptions.Default,
                    new object[] { log },
                    new TestAsyncInterceptor(log)),
            };

            return proxyFactories.Select(p => new object[] { p, new List<string>() });
        }

        [Theory]
        [MemberData(nameof(ClassProxyFactories))]
        public async Task ExtendClassProxyGenerator(
            Func<IProxyGenerator, List<string>, ClassWithVirtualMethodToProxy> proxyFactory,
            List<string> log)
        {
            // Act
            ClassWithVirtualMethodToProxy proxy = proxyFactory(Generator, log);
            Guid result = await proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            const string methodName = nameof(ClassWithVirtualMethodToProxy.AsynchronousResultMethod);
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal($"{methodName}:InterceptStart", log.First());
            Assert.Equal($"{methodName}:InterceptEnd", log.Last());
        }
    }
}
