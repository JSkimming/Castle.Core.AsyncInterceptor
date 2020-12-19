// Copyright (c) 2016-2020 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITarget
    {
        void VoidSynchronous();

        int ResultSynchronous();

        Task CompletedTaskAsynchronous();

        Task<int> CompletedResultTaskAsynchronous();

        Task IncompleteTaskAsynchronous();

        Task<int> IncompleteResultTaskAsynchronous();
    }
}
