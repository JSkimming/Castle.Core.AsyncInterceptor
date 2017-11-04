// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Castle.DynamicProxy
{
    public class Issue25MinimumRepro
    {
        [Fact]
        public async Task ShouldNotInterceptIndefinitely()
        {
            var interceptor = new Interceptor();
            ISample sample = new Sample();

            ProxyGenerator proxyGenerator = new ProxyGenerator();
            ISample proxy = proxyGenerator.CreateInterfaceProxyWithTargetInterface<ISample>(
                sample,
                interceptor);

            // Should not throw
            await proxy.DoAsync();
        }

        public interface ISample
        {
            Task DoAsync();
        }

        private class Sample : ISample
        {
            public Task DoAsync()
            {
                return Task.FromResult(0);
            }
        }
        
        /// <summary>
        /// A non-reusable interceptor for demonstration of Issue 25.
        /// The only proper usage is to use a single <see cref="Interceptor"/> ONE time
        /// to intercept ONE operation on the target interface.
        /// </summary>
        private class Interceptor : AsyncInterceptorBase
        {
            private int interceptions = 0;

            protected override async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
            {
                if (Interlocked.Increment(ref this.interceptions) > 1)
                {
                    throw new InvalidOperationException("InterceptAsync was called more than once");
                }

                await Task.Yield();
                await proceed(invocation);
            }

            protected override Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
            {
                throw new NotImplementedException();
            }
        }
    }
}
