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
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        public static IEnumerable<object[]> InterfaceProxyFactories()
        {
            Func<ProxyGenerator, List<string>, IInterfaceToProxy>[] proxyFactories =
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
            Func<ProxyGenerator, List<string>, IInterfaceToProxy> proxyFactory,
            List<string> log)
        {
            // Act
            IInterfaceToProxy proxy = proxyFactory(Generator, log);
            Guid result = await proxy.AsynchronousResultMethod();

            // Assert
            const string methodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(4, log.Count);
            Assert.Equal($"{methodName}:InterceptStart", log[0]);
            Assert.Equal($"{methodName}:InterceptEnd", log[3]);
        }
    }
}
