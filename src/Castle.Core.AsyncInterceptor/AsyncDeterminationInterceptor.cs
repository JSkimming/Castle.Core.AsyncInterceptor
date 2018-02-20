// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Castle.DynamicProxy.Invocations;

    /// <summary>
    /// Intercepts method invocations and determines if is an asynchronous method.
    /// </summary>
    public class AsyncDeterminationInterceptor : IInterceptor
    {
        private static readonly MethodInfo HandleAsyncFunctionMethodInfo =
            typeof(AsyncDeterminationInterceptor)
                    .GetMethod(nameof(HandleAsyncFunctionWithResult), BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly MethodInfo HandleFunctionMethodInfo =
            typeof(AsyncDeterminationInterceptor)
                .GetMethod(nameof(HandleFunctionWithResult), BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly ConcurrentDictionary<Type, GenericHandler> GenericAsyncFunctionHandlers =
            new ConcurrentDictionary<Type, GenericHandler>();

        private static readonly ConcurrentDictionary<Type, GenericHandler> GenericFunctionHandlers =
            new ConcurrentDictionary<Type, GenericHandler>();

        private readonly IAsyncInterceptor _asyncInterceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDeterminationInterceptor"/> class.
        /// </summary>
        public AsyncDeterminationInterceptor(IAsyncInterceptor asyncInterceptor)
        {
            _asyncInterceptor = asyncInterceptor;
        }

        private delegate void GenericHandler(IInvocation invocation, IAsyncInterceptor asyncInterceptor);

        private enum MethodType
        {
            Action,
            Function,
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
                case MethodType.Action:
                    _asyncInterceptor.InterceptAction(new ActionInvocation(invocation));
                    return;

                case MethodType.AsyncAction:
                    invocation.ReturnValue =
                        _asyncInterceptor.InterceptAsyncAction(new AsyncActionInvocation(invocation));
                    return;

                case MethodType.AsyncFunction:
                    GetAsyncFunctionHandler(invocation.Method.ReturnType).Invoke(invocation, _asyncInterceptor);
                    return;

                default:
                    GetFunctionHandler(invocation.Method.ReturnType).Invoke(invocation, _asyncInterceptor);
                    return;
            }
        }

        /// <summary>
        /// Gets the <see cref="MethodType"/> based upon the <paramref name="returnType"/> of the method invocation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MethodType GetMethodType(Type returnType)
        {
            // If there's no return type, or it's not a task, then assume it's a synchronous method.
            if (returnType == typeof(void))
                return MethodType.Action;

            if (returnType == typeof(Task))
                return MethodType.AsyncAction;

            return typeof(Task).IsAssignableFrom(returnType) ? MethodType.AsyncFunction : MethodType.Function;
        }

        /// <summary>
        /// Gets the <see cref="GenericHandler"/> for the method invocation <paramref name="returnType"/>.
        /// </summary>
        private static GenericHandler GetAsyncFunctionHandler(Type returnType)
        {
            return GenericAsyncFunctionHandlers.GetOrAdd(returnType, CreateAsyncFunctionHandler);
        }

        /// <summary>
        /// Gets the <see cref="GenericHandler"/> for the method invocation <paramref name="returnType"/>.
        /// </summary>
        private static GenericHandler GetFunctionHandler(Type returnType)
        {
            return GenericFunctionHandlers.GetOrAdd(returnType, CreateFunctionHandler);
        }

        /// <summary>
        /// Creates the generic delegate for the <paramref name="returnType"/> method invocation.
        /// </summary>
        private static GenericHandler CreateFunctionHandler(Type returnType)
        {
            MethodInfo method = HandleFunctionMethodInfo.MakeGenericMethod(returnType);
            return (GenericHandler)method.CreateDelegate(typeof(GenericHandler));
        }

        /// <summary>
        /// Creates the generic delegate for the <paramref name="returnType"/> method invocation.
        /// </summary>
        private static GenericHandler CreateAsyncFunctionHandler(Type returnType)
        {
            Type taskReturnType = returnType.GetGenericArguments()[0];
            MethodInfo method = HandleAsyncFunctionMethodInfo.MakeGenericMethod(taskReturnType);
            return (GenericHandler)method.CreateDelegate(typeof(GenericHandler));
        }

        /// <summary>
        /// This method is created as a delegate and used to make the call to the generic
        /// <see cref="IAsyncInterceptor.InterceptAsyncFunction{T}"/> method.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/> of the method
        /// <paramref name="invocation"/>.</typeparam>
        private static void HandleFunctionWithResult<TResult>(IInvocation invocation, IAsyncInterceptor asyncInterceptor)
        {
            invocation.ReturnValue =
                asyncInterceptor.InterceptFunction(new FunctionInvocation<TResult>(invocation));
        }

        /// <summary>
        /// This method is created as a delegate and used to make the call to the generic
        /// <see cref="IAsyncInterceptor.InterceptAsyncFunction{T}"/> method.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/> of the method
        /// <paramref name="invocation"/>.</typeparam>
        private static void HandleAsyncFunctionWithResult<TResult>(IInvocation invocation, IAsyncInterceptor asyncInterceptor)
        {
            invocation.ReturnValue =
                asyncInterceptor.InterceptAsyncFunction(new AsyncFunctionInvocation<TResult>(invocation));
        }
    }
}
