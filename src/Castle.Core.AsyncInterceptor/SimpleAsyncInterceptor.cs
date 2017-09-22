// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A base type for an <see cref="IAsyncInterceptor"/> to provided a simplified solution of method
    /// <see cref="IInvocation"/> by enforcing only two types of interception, both asynchronous.
    /// </summary>
    public abstract class SimpleAsyncInterceptor : IAsyncInterceptor
    {
#if !NETSTANDARD2_0
        /// <summary>
        /// A completed <see cref="Task"/>.
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(0);
#endif

        /// <summary>
        /// Intercepts a synchronous method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            Task task = InterceptAsync(invocation, ProceedSynchronous);

            // If the intercept task has yet to complete, wait for it.
            if (!task.IsCompleted)
            {
                Task.Run(() => task).Wait();
            }

            if (task.IsFaulted)
            {
                throw task.Exception.InnerException;
            }
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptAsync(invocation, ProceedAsynchronous);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptAsync(invocation, ProceedAsynchronous<TResult>);
        }

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceed">The function to proceed the <paramref name="invocation"/>.</param>
        /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
        protected abstract Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed);

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceed">The function to proceed the <paramref name="invocation"/>.</param>
        /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
        protected abstract Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation,
            Func<IInvocation, Task<TResult>> proceed);

        private static Task ProceedSynchronous(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
#if NETSTANDARD2_0
                return Task.CompletedTask;
#else
                return CompletedTask;
#endif
            }
            catch (Exception e)
            {
#if NETSTANDARD2_0
                return Task.FromException(e);
#else
                var tcs = new TaskCompletionSource<int>();
                tcs.SetException(e);
                return tcs.Task;
#endif
            }
        }

        private static async Task ProceedAsynchronous(IInvocation invocation)
        {
            invocation.Proceed();

            // Get the task to await.
            var originalReturnValue = (Task)invocation.ReturnValue;

            await originalReturnValue.ConfigureAwait(false);
        }

        private static async Task<TResult> ProceedAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.Proceed();

            // Get the task to await.
            var originalReturnValue = (Task<TResult>)invocation.ReturnValue;

            TResult result = await originalReturnValue.ConfigureAwait(false);
            return result;
        }
    }
}
