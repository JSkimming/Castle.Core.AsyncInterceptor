# Castle.Core.AsyncInterceptor

[![NuGet Version](https://img.shields.io/nuget/v/Castle.Core.AsyncInterceptor.svg)](https://www.nuget.org/packages/Castle.Core.AsyncInterceptor "NuGet Version")
[![Build status](https://ci.appveyor.com/api/projects/status/github/JSkimming/Castle.Core.AsyncInterceptor?branch=master&svg=true)](https://ci.appveyor.com/project/JSkimming/castle-core-asyncinterceptor "Build status")
[![Coverage Status](https://coveralls.io/repos/github/JSkimming/Castle.Core.AsyncInterceptor/badge.svg?branch=master&service=github)](https://coveralls.io/github/JSkimming/Castle.Core.AsyncInterceptor?branch=master "Coverage Status")
[![Latest release](https://img.shields.io/github/release/JSkimming/Castle.Core.AsyncInterceptor.svg)](https://github.com/JSkimming/Castle.Core.AsyncInterceptor/releases "Latest release")
<!--[![Coverity Scan Status](https://img.shields.io/coverity/scan/4829.svg)](https://scan.coverity.com/projects/4829 "Coverity Scan Status")-->

## What is AsyncInterceptor?

__AsyncInterceptor__ is an extension to [Castle DynamicProxy](http://www.castleproject.org/projects/dynamicproxy/) to
simplify the development of interceptors for asynchronous methods.

## Why do I want intercept methods?

The whys and wherefores of implementing interceptors is lengthy discussion, and beyond the scope of this introduction.
A very common scenario is in the implementation of
[Aspect-oriented patterns](https://en.wikipedia.org/wiki/Aspect-oriented_programming), for which exception handling is
useful use case.

An interceptor that catches exceptions and logs them could be implemented quite simply as:

```c#
// Intercept() is the single method of IInterceptor.
public void Intercept(IInvocation invocation)
{
    try
    {
        invocation.Proceed();
    }
    catch (Exception ex)
    {
        Log.Error($"Error calling {invocation.Method.Name}.", ex);
        throw;
    }
}
```

## What's not simple about asynchronous method interception?

When implementing `IInterceptor` the underlying the method is invoked like this:

```c#
public void Intercept(IInvocation invocation)
{
    // Step 1. Do something prior to invocation.

    invocation.Proceed();

    // Step 2. Do something after invocation.
}
```

For synchronous methods `Proceed()` returns only when the underlying method completes, but for asynchronous methods,
(those that return [`Task`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx) or
[`Task<TResult>`](https://msdn.microsoft.com/en-us/library/dd321424.aspx))
the `Proceed()` returns as soon as the underlying method hits an `await` (or `ContinueWith`).

Therefore with asynchronous methods _Step 2_ is executed before the underlying methods logically completes.

## How to intercept asynchronous methods without AsyncInterceptor?

To demonstrate how __AsyncInterceptor__ simplifies the interception of asynchronous methods, let's show how to do it
without __AsyncInterceptor__.

### Methods that return [`Task`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx)

To intercept methods that return a [`Task`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx)
(__Note:__ it must be a [`Task`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx) not
[`Task<TResult>`](https://msdn.microsoft.com/en-us/library/dd321424.aspx)) is not overly complicated.

The invocation provides access to the return value. By checking the type of the return value it is possible to await
the completion of the [`Task`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx).

```c#
public void Intercept(IInvocation invocation)
{
    // Step 1. Do something prior to invocation.

    invocation.Proceed();
    Type type = invocation.ReturnValue?.GetType();
    if (type != null && type == typeof(Task))
    {
        // Given the method returns a Task, wait for it to complete before performing Step 2
        Func<Task> continuation = async () =>
        {
            await (Task) invocation.ReturnValue;

            // Step 2. Do something after invocation.
        };

        invocation.ReturnValue = continuation();
        return;
    }

    // Assume the method is synchronous.

    // Step 2. Do something after invocation.
}
```

### Methods that return [`Task<TResult>`](https://msdn.microsoft.com/en-us/library/dd321424.aspx)

To intercept methods that return a [`Task<TResult>`](https://msdn.microsoft.com/en-us/library/dd321424.aspx) is far
from simple. It's the reason why I created this library. Rather than go into the detail (the solution requires the use
of reflection) the stack overflow answer to the question [Intercept async method that returns generic Task<> via
DynamicProxy](http://stackoverflow.com/a/28374134) provides great overview.

## How to intercept asynchronous methods __with__ AsyncInterceptor?

If you've got this far, then it's probably safe to assume you want to intercept asynchronous methods, and the options
for doing it manually look like a lot of work.

### Option 1:  Implement `IAsyncInterceptor` interface to intercept invocations

Create a class them implements `IAsyncInterceptor`, then register it for interception in the same was as `IInterceptor`
using the ProxyGenerator extension methods, e.g.

```c#
var myClass = new ClasThatImplementsIMyInterface();
var generator = new ProxyGenerator();
var interceptor = new ClasThatImplementsIAsyncInterceptor();
IMyInterface proxy = generator.CreateInterfaceProxyWithTargetInterface<IMyInterface>(myClass, interceptor)
```

Implementing
[IAsyncInterceptor](https://github.com/JSkimming/Castle.Core.AsyncInterceptor/blob/master/src/Castle.Core.AsyncInterceptor/IAsyncInterceptor.cs)
is the closest to traditional interception when implementing
[IInterceptor](https://github.com/castleproject/Core/blob/master/src/Castle.Core/DynamicProxy/IInterceptor.cs)

Instead of a single `void Intercept(IInvocation invocation)` method to implement, there are three:

```c#
void InterceptSynchronous(IInvocation invocation);
void InterceptAsynchronous(IInvocation invocation);
void InterceptAsynchronous<TResult>(IInvocation invocation);
```

#### `InterceptSynchronous(IInvocation invocation)`

`InterceptSynchronous` is effectively the same as `IInterceptor.Intercept`, though it is only called for synchronous
methods, e.g. methods that do not return `Task` or `Task<TResult>`.

Implementing `InterceptSynchronous` could look something like this:

```c#
public void InterceptSynchronous(IInvocation invocation)
{
    // Step 1. Do something prior to invocation.

    invocation.Proceed();

    // Step 2. Do something after invocation.
}
```

#### `InterceptAsynchronous(IInvocation invocation)`

`InterceptAsynchronous(IInvocation invocation)` is called for methods that return `Task` but not the generic
`TaskT<Result>`.

Implementing `InterceptAsynchronous(IInvocation invocation)` could look something like this:

```c#
public void InterceptAsynchronous(IInvocation invocation)
{
    invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
}

private async Task InternalInterceptAsynchronous(IInvocation invocation)
{
    // Step 1. Do something prior to invocation.

    invocation.Proceed();
    var task = (Task)invocation.ReturnValue;
    await task;

    // Step 2. Do something after invocation.
}
```

#### `InterceptAsynchronous<TResult>(IInvocation invocation)`

`InterceptAsynchronous<TResult>(IInvocation invocation)` is called for methods that return the generic `TaskT<Result>`.

Implementing `InterceptAsynchronous<TResult>(IInvocation invocation)` could look something like this:

```c#
public void InterceptAsynchronous<TResult>(IInvocation invocation)
{
    invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
}

private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
{
    // Step 1. Do something prior to invocation.

    invocation.Proceed();
    var task = (Task<TResult>)invocation.ReturnValue;
    TResult result = await task;

    // Step 2. Do something after invocation.

    return result;
}
```

### Option 2: Extend `ProcessingAsyncInterceptor<TState>` class to intercept invocations

Create a class that extends the abstract base class `ProcessingAsyncInterceptor<TState>`, then register it for
interception in the same was as `IInterceptor` using the ProxyGenerator extension methods, e.g.

```c#
var myClass = new ClasThatImplementsIMyInterface();
var generator = new ProxyGenerator();
var interceptor = new ClasThatExtendsProcessingAsyncInterceptor();
IMyInterface proxy = generator.CreateInterfaceProxyWithTargetInterface<IMyInterface>(myClass, interceptor)
```

Extending
[`ProcessingAsyncInterceptor<TState>`](https://github.com/JSkimming/Castle.Core.AsyncInterceptor/blob/documentation/src/Castle.Core.AsyncInterceptor/ProcessingAsyncInterceptor.cs),
provides a simplified mechanism of intercepting method invocations without having to implement the three methods of
`IAsyncInterceptor`.

`ProcessingAsyncInterceptor<TState>` defines two virtual methods, one that is invoked before to the method invocation,
the second after.

```c#
protected virtual TState StartingInvocation(IInvocation invocation);
protected virtual void CompletedInvocation(IInvocation invocation, TState state);
```

State can be maintained between the two method through the generic class parameter `TState`. `StartingInvocation` is
called before method invocation. The return value of type `TState` is then passed to the `CompletedInvocation` which is
called after method invocation.

A possible extension of `ProcessingAsyncInterceptor<TState>` could be as follows:

```c#
public class MyProcessingAsyncInterceptor : ProcessingAsyncInterceptor<string>
{
    protected override string StartingInvocation(IInvocation invocation)
    {
        return $"{invocation.Method.Name}:StartingInvocation:{DateTime.UtcNow:O}";
    }

    protected override void CompletedInvocation(IInvocation invocation, string state)
    {
        Trace.WriteLine(state);
        Trace.WriteLine($"{invocation.Method.Name}:CompletedInvocation:{DateTime.UtcNow:O}");
    }
}
```

The state of type `TState` returned from `StartingInvocation` can be `null`. Neither `StartingInvocation` nor
`CompletedInvocation` are require to be overridden in the class that derives from `ProcessingAsyncInterceptor<TState>`.
The default implementation of StartingInvocation simply returns `null`. If all you require is to intercept methods
after they are invoked, then just implement `CompletedInvocation` and ignore the `state` parameter which will be
null. In that situation your class can be defined as:


```c#
public class MyProcessingAsyncInterceptor : ProcessingAsyncInterceptor<object>
{
    protected override void CompletedInvocation(IInvocation invocation, object state)
    {
        Trace.WriteLine($"{invocation.Method.Name}:CompletedInvocation:{DateTime.UtcNow:O}");
    }
}
```

## Method invocation timing using `AsyncTimingInterceptor`

A common use-case for method invocation interception is to time how long a method takes to execute. For this reason
[`AsyncTimingInterceptor`](https://github.com/JSkimming/Castle.Core.AsyncInterceptor/blob/documentation/src/Castle.Core.AsyncInterceptor/AsyncTimingInterceptor.cs)
is provided.

`AsyncTimingInterceptor` is a specialised implementation of `ProcessingAsyncInterceptor<TState>` that uses a
[Stopwatch](https://msdn.microsoft.com/en-us/library/system.diagnostics.stopwatch.aspx) as the `TState`.

`AsyncTimingInterceptor` defines two abstract methods, one that is invoked before method invocation and before the
Stopwatch has started. The second after method invocation and the Stopwatch has stopped

```c#
protected abstract void StartingTiming(IInvocation invocation);
protected abstract void CompletedTiming(IInvocation invocation, Stopwatch stopwatch);
```

A possible extension of `AsyncTimingInterceptor` could be as follows:

```c#
public class TestAsyncTimingInterceptor : AsyncTimingInterceptor
{
    protected override void StartingTiming(IInvocation invocation)
    {
        Trace.WriteLine($"{invocation.Method.Name}:StartingTiming");
    }

    protected override void CompletedTiming(IInvocation invocation, Stopwatch stopwatch)
    {
        Trace.WriteLine($"{invocation.Method.Name}:CompletedTiming:{stopwatch.Elapsed:g}");
    }
}
```
