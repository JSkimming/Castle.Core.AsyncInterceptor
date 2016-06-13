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
        private readonly ICollection<string> _log;

        public ClassWithInterfaceToProxy(List<string> log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            _log = log;
        }

        public void SynchronousVoidMethod()
        {
            _log.Add($"{nameof(SynchronousVoidMethod)}:Start");
            Thread.Sleep(10);
            _log.Add($"{nameof(SynchronousVoidMethod)}:End");
        }

        public Guid SynchronousResultMethod()
        {
            _log.Add($"{nameof(SynchronousResultMethod)}:Start");
            Thread.Sleep(10);
            _log.Add($"{nameof(SynchronousResultMethod)}:End");
            return Guid.NewGuid();
        }

        public Task AsynchronousVoidMethod()
        {
            _log.Add($"{nameof(AsynchronousVoidMethod)}:Start");
            return Task.Delay(10).ContinueWith(t => _log.Add($"{nameof(AsynchronousVoidMethod)}:End"));
        }

        public async Task<Guid> AsynchronousResultMethod()
        {
            _log.Add($"{nameof(AsynchronousResultMethod)}:Start");
            await Task.Delay(10);
            _log.Add($"{nameof(AsynchronousResultMethod)}:End");
            return Guid.NewGuid();
        }
    }
}
