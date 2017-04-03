// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Intercepts method invocations and determines if is an asynchronous method.
    /// </summary>
    public class AsyncDeterminationInterceptor : IInterceptor
    {
        private static readonly MethodInfo HandleAsyncMethodInfo =
            typeof(AsyncDeterminationInterceptor)
                    .GetMethod(nameof(HandleAsyncWithResult), BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly ConcurrentDictionary<Type, GenericAsyncHandler> GenericAsyncHandlers =
            new ConcurrentDictionary<Type, GenericAsyncHandler>();

        private readonly IAsyncInterceptor _asyncInterceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDeterminationInterceptor"/> class.
        /// </summary>
        public AsyncDeterminationInterceptor(IAsyncInterceptor asyncInterceptor)
        {
            _asyncInterceptor = asyncInterceptor;
        }

        private delegate void GenericAsyncHandler(IInvocation invocation, IAsyncInterceptor asyncInterceptor);

        private enum MethodType
        {
            Synchronous,
            AsyncAction,
            AsyncFunction,
        }

        /// <summary>
        /// Intercepts a method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        [DebuggerStepThrough]
        public virtual void Intercept(IInvocation invocation)
        {
            MethodType methodType = GetMethodType(invocation.Method.ReturnType);

            switch (methodType)
            {
                case MethodType.AsyncAction:
                    _asyncInterceptor.InterceptAsynchronous(invocation);
                    return;
                case MethodType.AsyncFunction:
                    GetHandler(invocation.Method.ReturnType).Invoke(invocation, _asyncInterceptor);
                    return;
                default:
                    _asyncInterceptor.InterceptSynchronous(invocation);
                    return;
            }
        }

        /// <summary>
        /// Gets the <see cref="MethodType"/> based upon the <paramref name="returnType"/> of the method invocation.
        /// </summary>
        private static MethodType GetMethodType(Type returnType)
        {
            // If there's no return type, or it's not a task, then assume it's a synchronous method.
            if (returnType == typeof(void) || !typeof(Task).IsAssignableFrom(returnType))
                return MethodType.Synchronous;

            // The return type is a task of some sort, so assume it's asynchronous
            return returnType.GetTypeInfo().IsGenericType ? MethodType.AsyncFunction : MethodType.AsyncAction;
        }

        /// <summary>
        /// Gets the <see cref="GenericAsyncHandler"/> for the method invocation <paramref name="returnType"/>.
        /// </summary>
        private static GenericAsyncHandler GetHandler(Type returnType)
        {
            GenericAsyncHandler handler = GenericAsyncHandlers.GetOrAdd(returnType, CreateHandler);
            return handler;
        }

        /// <summary>
        /// Creates the generic delegate for the <paramref name="returnType"/> method invocation.
        /// </summary>
        private static GenericAsyncHandler CreateHandler(Type returnType)
        {
            Type taskReturnType = returnType.GetGenericArguments()[0];
            MethodInfo method = HandleAsyncMethodInfo.MakeGenericMethod(taskReturnType);
            return (GenericAsyncHandler)method.CreateDelegate(typeof(GenericAsyncHandler));
        }

        /// <summary>
        /// This method is created as a delegate and used to make the call to the generic
        /// <see cref="IAsyncInterceptor.InterceptAsynchronous{T}"/> method.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/> of the method
        /// <paramref name="invocation"/>.</typeparam>
        private static void HandleAsyncWithResult<TResult>(IInvocation invocation, IAsyncInterceptor asyncInterceptor)
        {
            asyncInterceptor.InterceptAsynchronous<TResult>(invocation);
        }
    }
}
