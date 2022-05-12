// Copyright (c) 2016-2022 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

public abstract class InterceptorBenchmarksBase
{
    protected static readonly IProxyGenerator Generator = new ProxyGenerator();

    /// <summary>
    /// A <see cref="Target"/> instance with no interception or wrapper.
    /// </summary>
    protected static readonly ITarget RawTarget = new Target();

    protected static readonly ITarget NopWrapperTarget = new TargetNopWrapper(RawTarget);

    protected static readonly ITarget AsyncWrapperTarget = new TargetAsyncWrapper(RawTarget);

    protected static readonly ITarget NopInterceptorTarget =
        Generator.CreateInterfaceProxyWithTargetInterface(RawTarget, new NopInterceptor());

    protected static readonly ITarget NopAsyncInterceptorTarget =
        Generator.CreateInterfaceProxyWithTargetInterface(RawTarget, new NopAsyncInterceptor());

    protected static readonly ITarget ProcessingAsyncInterceptorTarget =
        Generator.CreateInterfaceProxyWithTargetInterface(RawTarget, new ProcessingAsyncInterceptor());

    protected static readonly ITarget NopAsyncTimingInterceptorTarget =
        Generator.CreateInterfaceProxyWithTargetInterface(RawTarget, new NopAsyncTimingInterceptor());

    ////protected static readonly ITarget AsyncInterceptorBaseTarget =
    ////    Generator.CreateInterfaceProxyWithTargetInterface(RawTarget, new BenchmarkAsyncInterceptorBase());
}

public abstract class VoidBenchmarksBase : InterceptorBenchmarksBase
{
    [Benchmark]
    public void Raw() => Execute(RawTarget);

    [Benchmark]
    public void NopWrapper() => Execute(NopWrapperTarget);

    [Benchmark]
    public void AsyncWrapper() => Execute(AsyncWrapperTarget);

    [Benchmark(Baseline = true)]
    public void NopInterceptor() => Execute(NopInterceptorTarget);

    [Benchmark]
    public void NopAsyncInterceptor() => Execute(NopAsyncInterceptorTarget);

    [Benchmark]
    public void ProcessingAsyncInterceptor() => Execute(ProcessingAsyncInterceptorTarget);

    [Benchmark]
    public void NopAsyncTimingInterceptor() => Execute(NopAsyncTimingInterceptorTarget);

    ////[Benchmark]
    ////public void AsyncInterceptorBase() => Execute(AsyncInterceptorBaseTarget);

    protected abstract void Execute(ITarget target);
}

public abstract class TaskBenchmarksBase : InterceptorBenchmarksBase
{
    [Benchmark]
    public Task Raw() => ExecuteAsync(RawTarget);

    [Benchmark]
    public Task NopWrapper() => ExecuteAsync(NopWrapperTarget);

    [Benchmark]
    public Task AsyncWrapper() => ExecuteAsync(AsyncWrapperTarget);

    [Benchmark(Baseline = true)]
    public Task NopInterceptor() => ExecuteAsync(NopInterceptorTarget);

    [Benchmark]
    public Task NopAsyncInterceptor() => ExecuteAsync(NopAsyncInterceptorTarget);

    [Benchmark]
    public Task ProcessingAsyncInterceptor() => ExecuteAsync(ProcessingAsyncInterceptorTarget);

    [Benchmark]
    public Task NopAsyncTimingInterceptor() => ExecuteAsync(NopAsyncTimingInterceptorTarget);

    ////[Benchmark]
    ////public Task AsyncInterceptorBase() => ExecuteAsync(AsyncInterceptorBaseTarget);

    protected abstract Task ExecuteAsync(ITarget target);
}

////[ShortRunJob]
////public class VoidSynchronousInterceptorBenchmarks : VoidBenchmarksBase
////{
////    protected override void Execute(ITarget target) => target.VoidSynchronous();
////}

////[ShortRunJob]
////public class ResultSynchronousInterceptorBenchmarks : VoidBenchmarksBase
////{
////    protected override void Execute(ITarget target) => target.ResultSynchronous();
////}

////[ShortRunJob]
////public class CompletedTaskAsynchronousInterceptorBenchmarks : TaskBenchmarksBase
////{
////    protected override Task ExecuteAsync(ITarget target) => target.CompletedTaskAsynchronous();
////}

////[ShortRunJob]
////public class CompletedResultTaskAsynchronousInterceptorBenchmarks : TaskBenchmarksBase
////{
////    protected override Task ExecuteAsync(ITarget target) => target.CompletedResultTaskAsynchronous();
////}

////[ShortRunJob]
////public class IncompleteTaskAsynchronousInterceptorBenchmarks : TaskBenchmarksBase
////{
////    protected override Task ExecuteAsync(ITarget target) => target.IncompleteTaskAsynchronous();
////}

[ShortRunJob(RuntimeMoniker.Net472)]
[ShortRunJob(RuntimeMoniker.Net60)]
////[SimpleJob(Run)]
public class IncompleteResultTaskAsynchronousInterceptorBenchmarks : TaskBenchmarksBase
{
    [Params("IncompleteResult", "CompletedResult")]
    public string Method = string.Empty;

    protected override Task ExecuteAsync(ITarget target)
    {
        return Method switch
        {
            "CompletedResult" => target.CompletedResultTaskAsynchronous(),
            "IncompleteResult" => target.IncompleteResultTaskAsynchronous(),
            _ => Task.CompletedTask,
        };
    }
}
