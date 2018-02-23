// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.InterfaceProxies;
    using Xunit;

    public abstract class WhenExceptionInterceptingSynchronousVoidMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidExceptionMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        protected WhenExceptionInterceptingSynchronousVoidMethodsBase(int msDelay)
        {
            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
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
    }

    public class WhenExceptionInterceptingSynchronousVoidMethodsWithNoDelay
        : WhenExceptionInterceptingSynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingSynchronousVoidMethodsWithNoDelay() : base(0)
        {
        }
    }

    public class WhenExceptionInterceptingSynchronousVoidMethodsWithADelay
        : WhenExceptionInterceptingSynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingSynchronousVoidMethodsWithADelay() : base(10)
        {
        }
    }

    public abstract class WhenExceptionInterceptingSynchronousResultMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultExceptionMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        protected WhenExceptionInterceptingSynchronousResultMethodsBase(int msDelay)
        {
            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
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
    }

    public class WhenExceptionInterceptingSynchronousResultMethodsWithNoDelay
        : WhenExceptionInterceptingSynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingSynchronousResultMethodsWithNoDelay() : base(0)
        {
        }
    }

    public class WhenExceptionInterceptingSynchronousResultMethodsWithADelay
        : WhenExceptionInterceptingSynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingSynchronousResultMethodsWithADelay() : base(10)
        {
        }
    }

    public abstract class WhenExceptionInterceptingAsynchronousVoidMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidExceptionMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        protected WhenExceptionInterceptingAsynchronousVoidMethodsBase(int msDelay)
        {
            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
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
    }

    public class WhenExceptionInterceptingAsynchronousVoidMethodsWithNoDelay
        : WhenExceptionInterceptingAsynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousVoidMethodsWithNoDelay() : base(0)
        {
        }
    }

    public class WhenExceptionInterceptingAsynchronousVoidMethodsWithADelay
        : WhenExceptionInterceptingAsynchronousVoidMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousVoidMethodsWithADelay() : base(10)
        {
        }
    }

    public abstract class WhenExceptionInterceptingAsynchronousResultMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultExceptionMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        protected WhenExceptionInterceptingAsynchronousResultMethodsBase(int msDelay)
        {
            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
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
    }

    public class WhenExceptionInterceptingAsynchronousResultMethodsWithNoDelay
        : WhenExceptionInterceptingAsynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousResultMethodsWithNoDelay() : base(0)
        {
        }
    }

    public class WhenExceptionInterceptingAsynchronousResultMethodsWithADelay
        : WhenExceptionInterceptingAsynchronousResultMethodsBase
    {
        public WhenExceptionInterceptingAsynchronousResultMethodsWithADelay() : base(10)
        {
        }
    }

    /// <summary>
    /// The intention of this tests to validate the correct behavior when a synchronous exception is thrown from an
    /// asynchronous method, by this it means a method that returns Task or Task<TResult> not the async/await pattern.
    /// </summary>
    public class WhenExceptionInterceptingAnAsynchronousMethodThatThrowsASynchronousException
    {
        private class MyInterceptorBase : AsyncInterceptorBase
        {
            protected override Task InterceptAsync(IAsyncInvocation invocation, Func<Task> proceed)
            {
                return proceed();
            }

            protected override Task<TResult> InterceptAsync<TResult>(IAsyncInvocation invocation, Func<Task<TResult>> proceed)
            {
                return proceed();
            }
        }

        public class MyClass
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

        [Fact]
        public void ShouldThrowTheUnderlyingExceptionAndNotAFaultedTask()
        {
            // Arrange
            MyClass sut = ProxyGen.Generator.CreateClassProxyWithTarget(new MyClass(), new MyInterceptorBase());

            // Act
            Action action = () => sut.Test1();

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void ShouldThrowTheUnderlyingExceptionAndNotAFaultedResult()
        {
            // Arrange
            MyClass sut = ProxyGen.Generator.CreateClassProxyWithTarget(new MyClass(), new MyInterceptorBase());

            // Act
            Action action = () => sut.Test2();

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }
    }
}
