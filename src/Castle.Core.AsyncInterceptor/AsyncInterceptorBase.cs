// Copyright (c) 2016-2022 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Castle.DynamicProxy.NoCoverage;

/// <summary>
/// A base type for an <see cref="IAsyncInterceptor"/> to provided a simplified solution of method
/// <see cref="IInvocation"/> by enforcing only two types of interception, both asynchronous.
/// </summary>
[SuppressMessage(
    "Design", "CA1031:Do not catch general exception types", Justification = "Must propagate the same exceptions.")]
public abstract class AsyncInterceptorBase : IAsyncInterceptor
{
#if NET45
    /// <summary>
    /// A completed <see cref="Task"/>.
    /// </summary>
    private static readonly Task CompletedTask = Task.FromResult(0);
#endif

    private static readonly MethodInfo InterceptSynchronousMethodInfo =
        typeof(AsyncInterceptorBase).GetMethod(
            nameof(InterceptSynchronousResult), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static readonly ConcurrentDictionary<Type, GenericSynchronousHandler> GenericSynchronousHandlers =
        new ConcurrentDictionary<Type, GenericSynchronousHandler>
        {
            [typeof(void)] = InterceptSynchronousVoid,
        };

    private delegate void GenericSynchronousHandler(AsyncInterceptorBase me, IInvocation invocation);

    /// <summary>
    /// Intercepts a synchronous method <paramref name="invocation"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void InterceptSynchronous(IInvocation invocation)
    {
        Type returnType = invocation.Method.ReturnType;
        GenericSynchronousHandler handler = GenericSynchronousHandlers.GetOrAdd(returnType, CreateHandler);
        handler(this, invocation);
    }

    /// <summary>
    /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    public void InterceptAsynchronous(IInvocation invocation)
    {
        invocation.ReturnValue = InterceptAsync(invocation, invocation.CaptureProceedInfo(), ProceedAsynchronous);
    }

    /// <summary>
    /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
    /// <param name="invocation">The method invocation.</param>
    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        invocation.ReturnValue =
            InterceptAsync(invocation, invocation.CaptureProceedInfo(), ProceedAsynchronous<TResult>);
    }

    /// <summary>
    /// Override in derived classes to intercept method invocations.
    /// </summary>
    /// <param name="invocation">The method invocation.</param>
    /// <param name="proceedInfo">The <see cref="IInvocationProceedInfo"/>.</param>
    /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
    /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
    protected abstract Task InterceptAsync(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task> proceed);

    /// <summary>
    /// Override in derived classes to intercept method invocations.
    /// </summary>
    /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
    /// <param name="invocation">The method invocation.</param>
    /// <param name="proceedInfo">The <see cref="IInvocationProceedInfo"/>.</param>
    /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
    /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
    protected abstract Task<TResult> InterceptAsync<TResult>(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed);

    private static GenericSynchronousHandler CreateHandler(Type returnType)
    {
        MethodInfo method = InterceptSynchronousMethodInfo.MakeGenericMethod(returnType);
        return (GenericSynchronousHandler)method.CreateDelegate(typeof(GenericSynchronousHandler));
    }

    private static void InterceptSynchronousVoid(AsyncInterceptorBase me, IInvocation invocation)
    {
        Task task = me.InterceptAsync(invocation, invocation.CaptureProceedInfo(), ProceedSynchronous);

        // If the intercept task has yet to complete, wait for it.
        if (!task.IsCompleted)
        {
            // Need to use Task.Run() to prevent deadlock in .NET Framework ASP.NET requests.
            // GetAwaiter().GetResult() prevents a thrown exception being wrapped in a AggregateException.
            // See https://stackoverflow.com/a/17284612
            Task.Run(() => task).GetAwaiter().GetResult();
        }

        task.RethrowIfFaulted();
    }

    private static void InterceptSynchronousResult<TResult>(AsyncInterceptorBase me, IInvocation invocation)
    {
        Task<TResult> task = me.InterceptAsync(invocation, invocation.CaptureProceedInfo(), ProceedSynchronous<TResult>);

        // If the intercept task has yet to complete, wait for it.
        if (!task.IsCompleted)
        {
            // Need to use Task.Run() to prevent deadlock in .NET Framework ASP.NET requests.
            // GetAwaiter().GetResult() prevents a thrown exception being wrapped in a AggregateException.
            // See https://stackoverflow.com/a/17284612
            Task.Run(() => task).GetAwaiter().GetResult();
        }

        task.RethrowIfFaulted();
    }

    private static Task ProceedSynchronous(IInvocation invocation, IInvocationProceedInfo proceedInfo)
    {
        try
        {
            proceedInfo.Invoke();
#if NET45
            return CompletedTask;
#else
            return Task.CompletedTask;
#endif
        }
        catch (Exception e)
        {
#if NET45
            var tcs = new TaskCompletionSource<int>();
            tcs.SetException(e);
            return tcs.Task;
#else
            return Task.FromException(e);
#endif
        }
    }

    private static Task<TResult> ProceedSynchronous<TResult>(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo)
    {
        try
        {
            proceedInfo.Invoke();
            return Task.FromResult((TResult)invocation.ReturnValue);
        }
        catch (Exception e)
        {
#if NET45
            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(e);
            return tcs.Task;
#else
            return Task.FromException<TResult>(e);
#endif
        }
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "The name explicitly says Asynchronous.")]
    private static async Task ProceedAsynchronous(IInvocation invocation, IInvocationProceedInfo proceedInfo)
    {
        proceedInfo.Invoke();

        // Get the task to await.
        var originalReturnValue = (Task)invocation.ReturnValue;

        await originalReturnValue.ConfigureAwait(false);
    }

    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "The name explicitly says Asynchronous.")]
    private static async Task<TResult> ProceedAsynchronous<TResult>(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo)
    {
        proceedInfo.Invoke();

        // Get the task to await.
        var originalReturnValue = (Task<TResult>)invocation.ReturnValue;

        TResult result = await originalReturnValue.ConfigureAwait(false);
        return result;
    }
}
