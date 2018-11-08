// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ClassWithInterfaceToProxy : IInterfaceToProxy
    {
        private readonly List<string> _log;

        public ClassWithInterfaceToProxy(List<string> log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public IReadOnlyList<string> Log => _log;

        public void SynchronousVoidMethod()
        {
            _log.Add(nameof(SynchronousVoidMethod) + ":Start");
            Thread.Sleep(10);
            _log.Add(nameof(SynchronousVoidMethod) + ":End");
        }

        public void SynchronousVoidExceptionMethod()
        {
            _log.Add(nameof(SynchronousVoidExceptionMethod) + ":Start");
            Thread.Sleep(10);
            throw new InvalidOperationException(nameof(SynchronousVoidExceptionMethod) + ":Exception");
        }

        public Guid SynchronousResultMethod()
        {
            _log.Add(nameof(SynchronousResultMethod) + ":Start");
            Thread.Sleep(10);
            _log.Add(nameof(SynchronousResultMethod) + ":End");
            return Guid.NewGuid();
        }

        public Guid SynchronousResultExceptionMethod()
        {
            _log.Add(nameof(SynchronousResultExceptionMethod) + ":Start");
            Thread.Sleep(10);
            throw new InvalidOperationException(nameof(SynchronousResultExceptionMethod) + ":Exception");
        }

        public Task AsynchronousVoidMethod()
        {
            _log.Add(nameof(AsynchronousVoidMethod) + ":Start");
            return Task.Delay(10).ContinueWith(t => _log.Add(nameof(AsynchronousVoidMethod) + ":End"));
        }

        public Task AsynchronousVoidExceptionMethod()
        {
            _log.Add(nameof(AsynchronousVoidExceptionMethod) + ":Start");
            return Task.Delay(10).ContinueWith(
                t => throw new InvalidOperationException(nameof(AsynchronousVoidExceptionMethod) + ":Exception"));
        }

        public async Task<Guid> AsynchronousResultMethod()
        {
            _log.Add(nameof(AsynchronousResultMethod) + ":Start");
            await Task.Delay(10).ConfigureAwait(false);
            _log.Add(nameof(AsynchronousResultMethod) + ":End");
            return Guid.NewGuid();
        }

        public async Task<Guid> AsynchronousResultExceptionMethod()
        {
            _log.Add(nameof(AsynchronousResultExceptionMethod) + ":Start");
            await Task.Delay(10).ConfigureAwait(false);
            throw new InvalidOperationException(nameof(AsynchronousResultExceptionMethod) + ":Exception");
        }

    }
}
