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

When implementing `IInterceptor` the underlying the class is invoked like this:

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
from simple. It's the reason why I created this library. Rather than go into the detail (the solution requires using
reflection) the stack overflow answer to the question [Intercept async method that returns generic Task<> via
DynamicProxy](http://stackoverflow.com/a/28374134) provides great overview.
