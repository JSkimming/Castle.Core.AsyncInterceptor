// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;

    using Castle.DynamicProxy.Invocations;

    /// <inheritdoc cref="IInterceptor"/>
    internal class InterceptorAdapter : IInterceptor
    {
        private static readonly MethodInfo InterceptAsyncFunctionMethodInfo = typeof(InterceptorAdapter).GetMethod(
            nameof(ExecuteForAsyncFunction),
            BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo InterceptFunctionMethodInfo = typeof(InterceptorAdapter).GetMethod(
            nameof(ExecuteForFunction),
            BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly IAsyncInterceptor _interceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptorAdapter"/> class.
        /// </summary>
        /// <param name="interceptor">
        /// The interceptors.
        /// </param>
        public InterceptorAdapter(IAsyncInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        /// <summary>
        /// Intercepts a method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                if (invocation.Method.ReturnType == typeof(void))
                {
                    _interceptor.InterceptAction(new ActionInvocation(invocation));
                }
                else if (invocation.Method.ReturnType == typeof(Task))
                {
                    invocation.ReturnValue = _interceptor.InterceptAsyncAction(new AsyncActionInvocation(invocation));
                }
                else if (typeof(Task).IsAssignableFrom(invocation.Method.ReturnType))
                {
                    invocation.ReturnValue = InterceptAsyncFunctionMethodInfo
                        .MakeGenericMethod(invocation.Method.ReturnType.GetGenericArguments()[0])
                        .Invoke(this, new object[] { invocation });
                }
                else
                {
                    invocation.ReturnValue = InterceptFunctionMethodInfo.MakeGenericMethod(invocation.Method.ReturnType)
                        .Invoke(this, new object[] { invocation });
                }
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        /// <summary>
        /// Executes the AsyncFunction
        /// </summary>
        /// <typeparam name="TResult">Type of the return value</typeparam>
        /// <param name="invocation">Castle's invocation</param>
        /// <returns>The async result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Task<TResult> ExecuteForAsyncFunction<TResult>(IInvocation invocation)
        {
            return _interceptor.InterceptAsyncFunction(new AsyncFunctionInvocation<TResult>(invocation));
        }

        /// <summary>
        /// Executes the Function
        /// </summary>
        /// <typeparam name="TResult">Type of the return value</typeparam>
        /// <param name="invocation">Castle's invocation</param>
        /// <returns>The result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal TResult ExecuteForFunction<TResult>(IInvocation invocation)
        {
            return _interceptor.InterceptFunction(new FunctionInvocation<TResult>(invocation));
        }
    }
}
