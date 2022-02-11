// Copyright (c) 2016-2022 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.InterfaceProxies;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ClassWithAlwaysIncompleteAsync : IInterfaceToProxy
{
    private readonly ListLogger _log;

    public ClassWithAlwaysIncompleteAsync(ListLogger log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public IReadOnlyList<string> Log => _log.GetLog();

    public void SynchronousVoidMethod()
    {
        throw new NotImplementedException();
    }

    public void SynchronousVoidExceptionMethod()
    {
        throw new NotImplementedException();
    }

    public Guid SynchronousResultMethod()
    {
        throw new NotImplementedException();
    }

    public Guid SynchronousResultExceptionMethod()
    {
        throw new NotImplementedException();
    }

    public Task AsynchronousVoidExceptionMethod()
    {
        throw new NotImplementedException();
    }

    public Task<Guid> AsynchronousResultExceptionMethod()
    {
        throw new NotImplementedException();
    }

    public async Task AsynchronousVoidMethod()
    {
        _log.Add(nameof(AsynchronousVoidMethod) + ":Start");
        await Task.Delay(10).ConfigureAwait(false);
        _log.Add(nameof(AsynchronousVoidMethod) + ":End");
    }

    public async Task<Guid> AsynchronousResultMethod()
    {
        _log.Add(nameof(AsynchronousResultMethod) + ":Start");
        await Task.Delay(10).ConfigureAwait(false);
        _log.Add(nameof(AsynchronousResultMethod) + ":End");
        return Guid.NewGuid();
    }
}
