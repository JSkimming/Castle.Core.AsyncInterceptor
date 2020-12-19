// Copyright (c) 2016-2020 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Target : ITarget
    {
        private static readonly Task<int> CompletedResultTask = Task.FromResult(1);

        public void VoidSynchronous()
        {
        }

        public int ResultSynchronous() => 1;

        public Task CompletedTaskAsynchronous() => Task.CompletedTask;

        public Task<int> CompletedResultTaskAsynchronous() => CompletedResultTask;

        public async Task IncompleteTaskAsynchronous()
        {
            await Task.Yield();
        }

        public async Task<int> IncompleteResultTaskAsynchronous()
        {
            await Task.Yield();
            return 1;
        }
    }
}
