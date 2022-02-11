// Copyright (c) 2016-2022 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ClassWithVirtualMethodToProxy
{
    private ListLogger _log = null!;

    public ClassWithVirtualMethodToProxy(ListLogger log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    protected ClassWithVirtualMethodToProxy()
    {
    }

    public IReadOnlyList<string> Log => _log.GetLog();

    public virtual async Task<Guid> AsynchronousResultMethod()
    {
        _log.Add($"{nameof(AsynchronousResultMethod)}:Start");
        await Task.Delay(10).ConfigureAwait(false);
        _log.Add($"{nameof(AsynchronousResultMethod)}:End");
        return Guid.NewGuid();
    }

    internal void PostConstructorInitialize(ListLogger log)
    {
        _log ??= log ?? throw new ArgumentNullException(nameof(log));
    }
}
