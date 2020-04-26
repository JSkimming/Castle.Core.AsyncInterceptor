// Copyright (c) 2016-2020 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.InterfaceProxies;
    using Xunit;
    using Xunit.Abstractions;

    public abstract class InterceptExceptionBase
    {
        private readonly ITestOutputHelper _output;

        protected InterceptExceptionBase(ITestOutputHelper output)
        {
            _output = output;
        }

        public void CompareStackTrace(Exception? noneInterceptedException, Exception? interceptedException)
        {
            string noneInterceptedSt = noneInterceptedException?.StackTrace ?? string.Empty;
            string interceptedSt = interceptedException?.StackTrace ?? string.Empty;

            _output.WriteLine(
                $"None Intercepted Stack Trace:{Environment.NewLine}{noneInterceptedSt}");
            _output.WriteLine($"Intercepted Stack Trace:{Environment.NewLine}{interceptedSt}");

            string[] separator = { Environment.NewLine };
            string expected = noneInterceptedSt.Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
            string actual = interceptedSt.Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public abstract class WhenExceptionInterceptingSynchronousVoidMethodsBase : InterceptExceptionBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidExceptionMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;
        private readonly ClassWithInterfaceToProxy _target;

        protected WhenExceptionInterceptingSynchronousVoidMethodsBase(
            ITestOutputHelper output,
            bool asyncB4Proceed,
            int msDelayAfterProceed)
            : base(output)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed, msDelayAfterProceed);
            _proxy = ProxyGen.CreateProxy(_log, interceptor, out _target);
        }

        [Fact]
        public void ShouldLog3Entries()
        {
            // Act
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => _proxy.SynchronousVoidExceptionMethod());

            // Assert
            Assert.Equal(3, _log.Count);
            Assert.NotNull(ex);
            Assert.Equal(MethodName + ":Start", _log[1]);
            Assert.Equal(MethodName + ":Exception", ex.Message);
        }

        [Fact]
        public void ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            Assert.Throws<InvalidOperationException>(() => _proxy.SynchronousVoidExceptionMethod());

            // Assert
            Assert.Equal(MethodName + ":StartingVoidInvocation", _log[0]);
        }

        [Fact]
        public void ShouldAllowExceptionHandling()
        {
            // Act
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => _proxy.SynchronousVoidExceptionMethod());

            // Assert
            Assert.Equal(MethodName + ":VoidExceptionThrown:" + ex.Message, _log[2]);
        }

        [Fact]
        public void ShouldPreserveTheStackTrace()
        {
            // Arrange - This test does not care about the internal logging, so disable it to remove the noise.
            _log.Disable();

            // Act
            InvalidOperationException interceptedException =
                Assert.Throws<InvalidOperationException>(() => _proxy.SynchronousVoidExceptionMethod());

            // Assert

            // Get the exception without being intercepted, this is used to compare against the intercepted exception.
            InvalidOperationException noneInterceptedException =
                Assert.Throws<InvalidOperationException>(() => _target.SynchronousVoidExceptionMethod());

            CompareStackTrace(noneInterceptedException, interceptedException);
        }
    }

    public class WhenExceptionInterceptingSynchronousVoidMethodsWithAsyncB4AndNoDelay
        : WhenExceptionInterceptingSynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingSynchronousVoidMethodsWithAsyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenExceptionInterceptingSynchronousVoidMethodsWithSyncB4AndNoDelay
        : WhenExceptionInterceptingSynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingSynchronousVoidMethodsWithSyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenExceptionInterceptingSynchronousVoidMethodsWithAsyncB4AndADelay
        : WhenExceptionInterceptingSynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingSynchronousVoidMethodsWithAsyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenExceptionInterceptingSynchronousVoidMethodsWithSyncB4AndADelay
        : WhenExceptionInterceptingSynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingSynchronousVoidMethodsWithSyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 10)
        {
        }
    }

    public abstract class WhenExceptionInterceptingSynchronousResultMethodsBase : InterceptExceptionBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultExceptionMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;
        private readonly ClassWithInterfaceToProxy _target;

        protected WhenExceptionInterceptingSynchronousResultMethodsBase(
            ITestOutputHelper output,
            bool asyncB4Proceed,
            int msDelayAfterProceed)
            : base(output)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed, msDelayAfterProceed);
            _proxy = ProxyGen.CreateProxy(_log, interceptor, out _target);
        }

        [Fact]
        public void ShouldLog3Entries()
        {
            // Act
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => _proxy.SynchronousResultExceptionMethod());

            // Assert
            Assert.Equal(3, _log.Count);
            Assert.NotNull(ex);
            Assert.Equal(MethodName + ":Start", _log[1]);
            Assert.Equal(MethodName + ":Exception", ex.Message);
        }

        [Fact]
        public void ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            Assert.Throws<InvalidOperationException>(() => _proxy.SynchronousResultExceptionMethod());

            // Assert
            Assert.Equal(MethodName + ":StartingResultInvocation", _log[0]);
        }

        [Fact]
        public void ShouldAllowExceptionHandling()
        {
            // Act
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => _proxy.SynchronousResultExceptionMethod());

            // Assert
            Assert.Equal(MethodName + ":ResultExceptionThrown:" + ex.Message, _log[2]);
        }

        [Fact]
        public void ShouldPreserveTheStackTrace()
        {
            // Arrange - This test does not care about the internal logging, so disable it to remove the noise.
            _log.Disable();

            // Act
            InvalidOperationException interceptedException =
                Assert.Throws<InvalidOperationException>(() => _proxy.SynchronousResultExceptionMethod());

            // Assert

            // Get the exception without being intercepted, this is used to compare against the intercepted exception.
            InvalidOperationException noneInterceptedException =
                Assert.Throws<InvalidOperationException>(() => _target.SynchronousResultExceptionMethod());

            CompareStackTrace(noneInterceptedException, interceptedException);
        }
    }

    public class WhenExceptionInterceptingSynchronousResultMethodsWithAsyncB4AndNoDelay
        : WhenExceptionInterceptingSynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingSynchronousResultMethodsWithAsyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenExceptionInterceptingSynchronousResultMethodsWithSyncB4AndNoDelay
        : WhenExceptionInterceptingSynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingSynchronousResultMethodsWithSyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenExceptionInterceptingSynchronousResultMethodsWithAsyncB4AndADelay
        : WhenExceptionInterceptingSynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingSynchronousResultMethodsWithAsyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenExceptionInterceptingSynchronousResultMethodsWithSyncB4AndADelay
        : WhenExceptionInterceptingSynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingSynchronousResultMethodsWithSyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 10)
        {
        }
    }

    public abstract class WhenExceptionInterceptingAsynchronousVoidMethodsBase : InterceptExceptionBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidExceptionMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;
        private readonly ClassWithInterfaceToProxy _target;

        protected WhenExceptionInterceptingAsynchronousVoidMethodsBase(
            ITestOutputHelper output,
            bool asyncB4Proceed,
            int msDelayAfterProceed)
            : base(output)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed, msDelayAfterProceed);
            _proxy = ProxyGen.CreateProxy(_log, interceptor, out _target);
        }

        [Fact]
        public async Task ShouldLog3Entries()
        {
            // Act
            InvalidOperationException ex =
                await Assert.ThrowsAsync<InvalidOperationException>(_proxy.AsynchronousVoidExceptionMethod)
                    .ConfigureAwait(false);

            // Assert
            Assert.Equal(3, _log.Count);
            Assert.NotNull(ex);
            Assert.Equal(MethodName + ":Start", _log[1]);
            Assert.Equal(MethodName + ":Exception", ex.Message);
        }

        [Fact]
        public async Task ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(_proxy.AsynchronousVoidExceptionMethod)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(MethodName + ":StartingVoidInvocation", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowExceptionHandling()
        {
            // Act
            InvalidOperationException ex =
                await Assert.ThrowsAsync<InvalidOperationException>(_proxy.AsynchronousVoidExceptionMethod)
                    .ConfigureAwait(false);

            // Assert
            Assert.Equal(MethodName + ":VoidExceptionThrown:" + ex.Message, _log[2]);
        }

        [Fact]
        public async Task ShouldPreserveTheStackTrace()
        {
            // Arrange - This test does not care about the internal logging, so disable it to remove the noise.
            _log.Disable();

            // Act
            InvalidOperationException interceptedException =
                await Assert.ThrowsAsync<InvalidOperationException>(_proxy.AsynchronousVoidExceptionMethod)
                    .ConfigureAwait(false);

            // Assert

            // Get the exception without being intercepted, this is used to compare against the intercepted exception.
            InvalidOperationException noneInterceptedException =
                await Assert.ThrowsAsync<InvalidOperationException>(_target.AsynchronousVoidExceptionMethod)
                    .ConfigureAwait(false);

            CompareStackTrace(noneInterceptedException, interceptedException);
        }
    }

    public class WhenExceptionInterceptingAsynchronousVoidMethodsWithAsyncB4AndNoDelay
        : WhenExceptionInterceptingAsynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousVoidMethodsWithAsyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenExceptionInterceptingAsynchronousVoidMethodsWithSyncB4AndNoDelay
        : WhenExceptionInterceptingAsynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousVoidMethodsWithSyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenExceptionInterceptingAsynchronousVoidMethodsWithAsyncB4AndADelay
        : WhenExceptionInterceptingAsynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousVoidMethodsWithAsyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenExceptionInterceptingAsynchronousVoidMethodsWithSyncB4AndADelay
        : WhenExceptionInterceptingAsynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousVoidMethodsWithSyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 10)
        {
        }
    }

    public abstract class WhenExceptionInterceptingAsynchronousResultMethodsBase : InterceptExceptionBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultExceptionMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;
        private readonly ClassWithInterfaceToProxy _target;

        protected WhenExceptionInterceptingAsynchronousResultMethodsBase(
            ITestOutputHelper output,
            bool asyncB4Proceed,
            int msDelayAfterProceed)
            : base(output)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed, msDelayAfterProceed);
            _proxy = ProxyGen.CreateProxy(_log, interceptor, out _target);
        }

        [Fact]
        public async Task ShouldLog3Entries()
        {
            // Act
            InvalidOperationException ex =
                await Assert.ThrowsAsync<InvalidOperationException>(_proxy.AsynchronousResultExceptionMethod)
                    .ConfigureAwait(false);

            // Assert
            Assert.Equal(3, _log.Count);
            Assert.NotNull(ex);
            Assert.Equal(MethodName + ":Start", _log[1]);
            Assert.Equal(MethodName + ":Exception", ex.Message);
        }

        [Fact]
        public async Task ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(_proxy.AsynchronousResultExceptionMethod)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(MethodName + ":StartingResultInvocation", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowExceptionHandling()
        {
            // Act
            InvalidOperationException ex =
                await Assert.ThrowsAsync<InvalidOperationException>(_proxy.AsynchronousResultExceptionMethod)
                    .ConfigureAwait(false);

            // Assert
            Assert.Equal(MethodName + ":ResultExceptionThrown:" + ex.Message, _log[2]);
        }

        [Fact]
        public async Task ShouldPreserveTheStackTrace()
        {
            // Arrange - This test does not care about the internal logging, so disable it to remove the noise.
            _log.Disable();

            // Act
            InvalidOperationException interceptedException =
                await Assert.ThrowsAsync<InvalidOperationException>(_proxy.AsynchronousResultExceptionMethod)
                    .ConfigureAwait(false);

            // Assert

            // Get the exception without being intercepted, this is used to compare against the intercepted exception.
            InvalidOperationException noneInterceptedException =
                await Assert.ThrowsAsync<InvalidOperationException>(_target.AsynchronousResultExceptionMethod)
                    .ConfigureAwait(false);

            CompareStackTrace(noneInterceptedException, interceptedException);
        }
    }

    public class WhenExceptionInterceptingAsynchronousResultMethodsWithAsyncB4AndNoDelay
        : WhenExceptionInterceptingAsynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousResultMethodsWithAsyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenExceptionInterceptingAsynchronousResultMethodsWithSyncB4AndNoDelay
        : WhenExceptionInterceptingAsynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousResultMethodsWithSyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenExceptionInterceptingAsynchronousResultMethodsWithAsyncB4AndADelay
        : WhenExceptionInterceptingAsynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousResultMethodsWithAsyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenExceptionInterceptingAsynchronousResultMethodsWithSyncB4AndADelay
        : WhenExceptionInterceptingAsynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousResultMethodsWithSyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenExceptionInterceptingAnAsynchronousMethodThatThrowsASynchronousException : InterceptExceptionBase
    {
        public WhenExceptionInterceptingAnAsynchronousMethodThatThrowsASynchronousException(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public void ShouldReturnAFaultedTask()
        {
            // Arrange
            var target = new AllExceptions();
            AllExceptions sut = ProxyGen.Generator.CreateClassProxyWithTarget(target, new MyInterceptorBase());

            // Act
            Task result = sut.Test1();

            // Assert
            Assert.True(result.IsFaulted);
            Assert.IsType<ArgumentOutOfRangeException>(result.Exception?.InnerException);

            ArgumentOutOfRangeException noneInterceptedException =
                Assert.Throws<ArgumentOutOfRangeException>(() => { target.Test1(); });

            CompareStackTrace(noneInterceptedException, result.Exception?.InnerException);
        }

        [Fact]
        public void ShouldReturnAFaultedTaskResult()
        {
            // Arrange
            var target = new AllExceptions();
            AllExceptions sut = ProxyGen.Generator.CreateClassProxyWithTarget(target, new MyInterceptorBase());

            // Act
            Task<object> result = sut.Test2();

            // Assert
            Assert.True(result.IsFaulted);
            Assert.IsType<ArgumentOutOfRangeException>(result.Exception?.InnerException);

            ArgumentOutOfRangeException noneInterceptedException =
                Assert.Throws<ArgumentOutOfRangeException>(() => { target.Test2(); });

            CompareStackTrace(noneInterceptedException, result.Exception?.InnerException);
        }

        [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Just Testing.")]
        public class AllExceptions
        {
            public virtual Task Test1()
            {
                throw new ArgumentOutOfRangeException();
            }

            public virtual Task<object> Test2()
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private class MyInterceptorBase : AsyncInterceptorBase
        {
            protected override Task InterceptAsync(
                IInvocation invocation,
                IInvocationProceedInfo proceedInfo,
                Func<IInvocation, IInvocationProceedInfo, Task> proceed)
            {
                return proceed(invocation, proceedInfo);
            }

            protected override Task<TResult> InterceptAsync<TResult>(
                IInvocation invocation,
                IInvocationProceedInfo proceedInfo,
                Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
            {
                return proceed(invocation, proceedInfo);
            }
        }
    }
}
