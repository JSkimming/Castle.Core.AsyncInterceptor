// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

/// <summary>
/// Extension methods to <see cref="IProxyGenerator"/>.
/// </summary>
public static class ProxyGeneratorExtensions
{
    /// <summary>
    /// Creates an <see cref="IInterceptor"/> for the supplied <paramref name="interceptor"/>.
    /// </summary>
    /// <param name="interceptor">The interceptor for asynchronous operations.</param>
    /// <returns>The <see cref="IInterceptor"/> for the supplied <paramref name="interceptor"/>.</returns>
    public static IInterceptor ToInterceptor(this IAsyncInterceptor interceptor)
    {
        return new AsyncDeterminationInterceptor(interceptor);
    }

    /// <summary>
    /// Creates an array of <see cref="IInterceptor"/> objects for the supplied <paramref name="interceptors"/>.
    /// </summary>
    /// <param name="interceptors">The interceptors for asynchronous operations.</param>
    /// <returns>The <see cref="IInterceptor"/> array for the supplied <paramref name="interceptors"/>.</returns>
    public static IInterceptor[] ToInterceptors(this IEnumerable<IAsyncInterceptor> interceptors)
    {
        return interceptors.Select(ToInterceptor).ToArray();
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTarget{TInterface}(TInterface,IInterceptor[])" />
    public static TInterface CreateInterfaceProxyWithTarget<TInterface>(
        this IProxyGenerator proxyGenerator,
        TInterface target,
        params IAsyncInterceptor[] interceptors)
        where TInterface : class
    {
        return proxyGenerator.CreateInterfaceProxyWithTarget(target, interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTarget{TInterface}(TInterface, IInterceptor[])" />
    public static TInterface CreateInterfaceProxyWithTarget<TInterface>(
        this IProxyGenerator proxyGenerator,
        TInterface target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
        where TInterface : class
    {
        return proxyGenerator.CreateInterfaceProxyWithTarget(target, options, interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTarget(Type, object, IInterceptor[])" />
    public static object CreateInterfaceProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type interfaceToProxy,
        object target,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateInterfaceProxyWithTarget(
            interfaceToProxy,
            target,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTarget(Type, object, ProxyGenerationOptions, IInterceptor[])" />
    public static object CreateInterfaceProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type interfaceToProxy,
        object target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateInterfaceProxyWithTarget(
            interfaceToProxy,
            target,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTarget(Type, Type[], object, IInterceptor[])" />
    public static object CreateInterfaceProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type interfaceToProxy,
        Type[] additionalInterfacesToProxy,
        object target,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateInterfaceProxyWithTarget(
            interfaceToProxy,
            additionalInterfacesToProxy,
            target,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTarget(Type, Type[], object, ProxyGenerationOptions, IInterceptor[])" />
    public static object CreateInterfaceProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type interfaceToProxy,
        Type[] additionalInterfacesToProxy,
        object target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateInterfaceProxyWithTarget(
            interfaceToProxy,
            additionalInterfacesToProxy,
            target,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface(Type, object, IInterceptor[])" />
    public static object CreateInterfaceProxyWithTargetInterface(
        this IProxyGenerator proxyGenerator,
        Type interfaceToProxy,
        object target,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateInterfaceProxyWithTargetInterface(
            interfaceToProxy,
            target,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface{TInterface}(TInterface, IInterceptor[])" />
    public static TInterface CreateInterfaceProxyWithTargetInterface<TInterface>(
        this IProxyGenerator proxyGenerator,
        TInterface target,
        params IAsyncInterceptor[] interceptors)
        where TInterface : class
    {
        return proxyGenerator.CreateInterfaceProxyWithTargetInterface(target, interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface{TInterface}(TInterface, ProxyGenerationOptions, IInterceptor[])" />
    public static TInterface CreateInterfaceProxyWithTargetInterface<TInterface>(
        this IProxyGenerator proxyGenerator,
        TInterface target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
        where TInterface : class
    {
        return proxyGenerator.CreateInterfaceProxyWithTargetInterface(
            target,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface(Type, Type[], object, IInterceptor[])" />
    public static object CreateInterfaceProxyWithTargetInterface(
        this IProxyGenerator proxyGenerator,
        Type interfaceToProxy,
        Type[] additionalInterfacesToProxy,
        object target,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateInterfaceProxyWithTargetInterface(
            interfaceToProxy,
            additionalInterfacesToProxy,
            target,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface(Type, object, ProxyGenerationOptions, IInterceptor[])" />
    public static object CreateInterfaceProxyWithTargetInterface(
        this IProxyGenerator proxyGenerator,
        Type interfaceToProxy,
        object target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateInterfaceProxyWithTargetInterface(
            interfaceToProxy,
            target,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface(Type, Type[], object, ProxyGenerationOptions, IInterceptor[])" />
    public static object CreateInterfaceProxyWithTargetInterface(
        this IProxyGenerator proxyGenerator,
        Type interfaceToProxy,
        Type[] additionalInterfacesToProxy,
        object target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateInterfaceProxyWithTargetInterface(
            interfaceToProxy,
            additionalInterfacesToProxy,
            target,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget{TClass}(TClass, IInterceptor[])" />
    public static TClass CreateClassProxyWithTarget<TClass>(
        this IProxyGenerator proxyGenerator,
        TClass target,
        params IAsyncInterceptor[] interceptors)
        where TClass : class
    {
        return proxyGenerator.CreateClassProxyWithTarget(target, interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget{TClass}(TClass, ProxyGenerationOptions, IInterceptor[])" />
    public static TClass CreateClassProxyWithTarget<TClass>(
        this IProxyGenerator proxyGenerator,
        TClass target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
        where TClass : class
    {
        return proxyGenerator.CreateClassProxyWithTarget(target, options, interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget(Type, Type[], object, IInterceptor[])" />
    public static object CreateClassProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        Type[] additionalInterfacesToProxy,
        object target,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxyWithTarget(
            classToProxy,
            additionalInterfacesToProxy,
            target,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget(Type, object, ProxyGenerationOptions, object[], IInterceptor[])" />
    public static object CreateClassProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        object target,
        ProxyGenerationOptions options,
        object[] constructorArguments,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxyWithTarget(
            classToProxy,
            target,
            options,
            constructorArguments,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget(Type, object, object[], IInterceptor[])" />
    public static object CreateClassProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        object target,
        object[] constructorArguments,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxyWithTarget(
            classToProxy,
            target,
            constructorArguments,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget(Type, object, IInterceptor[])" />
    public static object CreateClassProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        object target,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxyWithTarget(classToProxy, target, interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget(Type, object, ProxyGenerationOptions, IInterceptor[])" />
    public static object CreateClassProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        object target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxyWithTarget(
            classToProxy,
            target,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget(Type, Type[], object, ProxyGenerationOptions, IInterceptor[])" />
    public static object CreateClassProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        Type[] additionalInterfacesToProxy,
        object target,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxyWithTarget(
            classToProxy,
            additionalInterfacesToProxy,
            target,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxyWithTarget(Type, Type[], object, ProxyGenerationOptions, object[], IInterceptor[])" />
    public static object CreateClassProxyWithTarget(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        Type[] additionalInterfacesToProxy,
        object target,
        ProxyGenerationOptions options,
        object[] constructorArguments,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxyWithTarget(
            classToProxy,
            additionalInterfacesToProxy,
            target,
            options,
            constructorArguments,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy{TClass}(IInterceptor[])" />
    public static TClass CreateClassProxy<TClass>(
        this IProxyGenerator proxyGenerator,
        params IAsyncInterceptor[] interceptors)
        where TClass : class
    {
        return proxyGenerator.CreateClassProxy<TClass>(interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy{TClass}(ProxyGenerationOptions, IInterceptor[])" />
    public static TClass CreateClassProxy<TClass>(
        this IProxyGenerator proxyGenerator,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
        where TClass : class
    {
        return proxyGenerator.CreateClassProxy<TClass>(options, interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy(Type, Type[], IInterceptor[])" />
    public static object CreateClassProxy(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        Type[] additionalInterfacesToProxy,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxy(
            classToProxy,
            additionalInterfacesToProxy,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy(Type, ProxyGenerationOptions, object[], IInterceptor[])" />
    public static object CreateClassProxy(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        ProxyGenerationOptions options,
        object[] constructorArguments,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxy(
            classToProxy,
            options,
            constructorArguments,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy(Type, object[], IInterceptor[])" />
    public static object CreateClassProxy(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        object[] constructorArguments,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxy(
            classToProxy,
            constructorArguments,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy(Type, IInterceptor[])" />
    public static object CreateClassProxy(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxy(classToProxy, interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy(Type, ProxyGenerationOptions, IInterceptor[])" />
    public static object CreateClassProxy(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxy(
            classToProxy,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy(Type, Type[], ProxyGenerationOptions, IInterceptor[])" />
    public static object CreateClassProxy(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        Type[] additionalInterfacesToProxy,
        ProxyGenerationOptions options,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxy(
            classToProxy,
            additionalInterfacesToProxy,
            options,
            interceptors.ToInterceptors());
    }

    /// <inheritdoc cref="IProxyGenerator.CreateClassProxy(Type, Type[], ProxyGenerationOptions, object[], IInterceptor[])" />
    public static object CreateClassProxy(
        this IProxyGenerator proxyGenerator,
        Type classToProxy,
        Type[] additionalInterfacesToProxy,
        ProxyGenerationOptions options,
        object[] constructorArguments,
        params IAsyncInterceptor[] interceptors)
    {
        return proxyGenerator.CreateClassProxy(
            classToProxy,
            additionalInterfacesToProxy,
            options,
            constructorArguments,
            interceptors.ToInterceptors());
    }
}
