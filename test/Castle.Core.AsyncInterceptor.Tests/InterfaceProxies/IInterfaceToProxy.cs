// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInterfaceToProxy
    {
        IReadOnlyList<string> Log { get; }

        void SynchronousVoidMethod();

        Guid SynchronousResultMethod();

        Task AsynchronousVoidMethod();

        Task<Guid> AsynchronousResultMethod();
    }
}
