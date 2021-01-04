// Copyright (c) 2016-2021 James Skimming. All rights reserved.
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

        void SynchronousVoidExceptionMethod();

        Guid SynchronousResultMethod();

        Guid SynchronousResultExceptionMethod();

        Task AsynchronousVoidMethod();

        Task AsynchronousVoidExceptionMethod();

        Task<Guid> AsynchronousResultMethod();

        Task<Guid> AsynchronousResultExceptionMethod();
    }
}
