// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System.Diagnostics;

    /// <summary>
    /// A base type for an <see cref="IAsyncInterceptor"/> which only wants timings for a method
    /// <see cref="IInvocation"/>.
    /// </summary>
    public abstract class AsyncTimingInterceptor : ProcessingAsyncInterceptor<Stopwatch>
    {
        /// <summary>
        /// Signals <see cref="StartingTiming"/> before starting a <see cref="Stopwatch"/> to time the method
        /// <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <returns>The <see cref="Stopwatch"/> to time the method <paramref name="invocation"/>.</returns>
        protected sealed override Stopwatch StartingInvocation(IInvocation invocation)
        {
            StartingTiming(invocation);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        /// <summary>
        /// Signals <see cref="CompletedTiming"/> after stopping a <paramref name="stopwatch"/> to time the method
        /// <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="stopwatch">The <see cref="Stopwatch"/> returned by <see cref="StartingInvocation"/> to time
        /// the method <paramref name="invocation"/>.</param>
        protected sealed override void CompletedInvocation(IInvocation invocation, Stopwatch stopwatch)
        {
            stopwatch.Stop();
            CompletedTiming(invocation, stopwatch);
        }

        /// <summary>
        /// Override in derived classes to receive signals prior method <paramref name="invocation"/> timing.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        protected abstract void StartingTiming(IInvocation invocation);

        /// <summary>
        /// Override in derived classes to receive signals after method <paramref name="invocation"/> timing.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="stopwatch">A <see cref="Stopwatch"/> used to time the method <paramref name="invocation"/>.
        /// </param>
        protected abstract void CompletedTiming(IInvocation invocation, Stopwatch stopwatch);
    }
}
