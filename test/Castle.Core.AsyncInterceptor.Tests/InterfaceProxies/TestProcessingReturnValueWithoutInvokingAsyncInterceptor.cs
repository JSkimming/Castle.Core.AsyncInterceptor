// Copyright (c) 2016-2023 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies;

using System.Reflection;

public class TestProcessingReturnValueWithoutInvokingAsyncInterceptor : AsyncInterceptorBase
{
    private static readonly MethodInfo TaskFromResultMethod = typeof(Task)
        .GetMethod(nameof(Task.FromResult), BindingFlags.Static | BindingFlags.Public)!;

    private readonly ListLogger _log;

    public TestProcessingReturnValueWithoutInvokingAsyncInterceptor(ListLogger log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    protected override Task InterceptAsync(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        try
        {
            _log.Add($"{invocation.Method.Name}:StartingVoidInvocation");

            /* Without invoking original method
            await proceed(invocation, proceedInfo).ConfigureAwait(false);
            */

            _log.Add($"{invocation.Method.Name}:CompletedVoidInvocation");

            // There is no async call in this example so we just return a completed task without async.
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            _log.Add($"{invocation.Method.Name}:VoidExceptionThrown:{e.Message}");
            throw;
        }
    }

    protected override Task<TResult> InterceptAsync<TResult>(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        try
        {
            _log.Add($"{invocation.Method.Name}:StartingResultInvocation");

            /* Without invoking original method
            TResult result = await proceed(invocation, proceedInfo).ConfigureAwait(false);
            */

            // But we need a default result
            TResult? result = GetDefaultValue<TResult>();

            _log.Add($"{invocation.Method.Name}:CompletedResultInvocation:{result}");

            // Add ! here because the return value type in the base definition is supposed to be a nullable reference
            // but it isn't
            return Task.FromResult(result!);
        }
        catch (Exception e)
        {
            _log.Add($"{invocation.Method.Name}:VoidExceptionThrown:{e.Message}");
            throw;
        }
    }

    private TResult? GetDefaultValue<TResult>()
    {
        return (TResult?)GetDefaultValue(typeof(TResult));
    }

    private object? GetDefaultValue(Type type)
    {
        if (type.IsAssignableFrom(typeof(Task)))
        {
            if (type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments().Single();
                object? innerResult = GetDefaultValue(innerType);
                MethodInfo fromResult = TaskFromResultMethod.MakeGenericMethod(innerType);
                return fromResult.Invoke(null, new[] { innerResult });
            }

            return Task.CompletedTask;
        }

        if (type.IsValueType) return Activator.CreateInstance(type);

        return null;
    }
}
