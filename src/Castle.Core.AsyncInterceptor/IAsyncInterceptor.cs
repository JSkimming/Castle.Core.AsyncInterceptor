// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

/// <summary>
/// Implement this interface to intercept method invocations with DynamicProxy2.
/// </summary>
public interface IAsyncInterceptor
{
    /// <summary>
    /// Intercepts a synchronous method <paramref name="invocation"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    void InterceptSynchronous(IInvocation invocation);

    /// <summary>
    /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    void InterceptAsynchronous(IInvocation invocation);

    /// <summary>
    /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
    /// <param name="invocation">The method invocation.</param>
    void InterceptAsynchronous<TResult>(IInvocation invocation);

#if NET5_0_OR_GREATER
    /// <summary>
    /// Intercepts an asynchronous enumerable method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the <see cref="IAsyncEnumerable{T}"/>.</typeparam>
    /// <param name="invocation">The method invocation.</param>
    void InterceptAsynchronousEnumerable<TResult>(IInvocation invocation);
#endif
}
