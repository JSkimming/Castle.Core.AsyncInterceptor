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

    public abstract class WhenSimpleInterceptingSynchronousVoidMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        protected WhenSimpleInterceptingSynchronousVoidMethodsBase(int msDelay)
        {
            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestSimpleAsyncInterceptor(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
        }

        [Fact]
        public void ShouldLog4Entries()
        {
            // Act
            _proxy.SynchronousVoidMethod();

            // Assert
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public void ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            _proxy.SynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:StartingVoidInvocation", _log[0]);
        }

        [Fact]
        public void ShouldAllowProcessingAfterInvocation()
        {
            // Act
            _proxy.SynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedVoidInvocation", _log[3]);
        }
    }

    public class WhenSimpleInterceptingSynchronousVoidMethodsWithNoDelay
        : WhenSimpleInterceptingSynchronousVoidMethodsBase
    {
        public WhenSimpleInterceptingSynchronousVoidMethodsWithNoDelay() : base(0)
        {
        }
    }

    public class WhenSimpleInterceptingSynchronousVoidMethodsWithADelay
        : WhenSimpleInterceptingSynchronousVoidMethodsBase
    {
        public WhenSimpleInterceptingSynchronousVoidMethodsWithADelay() : base(10)
        {
        }
    }

    public abstract class WhenSimpleInterceptingSynchronousResultMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        protected WhenSimpleInterceptingSynchronousResultMethodsBase(int msDelay)
        {
            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestSimpleAsyncInterceptor(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
        }

        [Fact]
        public void ShouldLog4Entries()
        {
            // Act
            _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public void ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:StartingVoidInvocation", _log[0]);
        }

        [Fact]
        public void ShouldAllowProcessingAfterInvocation()
        {
            // Act
            _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedVoidInvocation", _log[3]);
        }
    }

    public class WhenSimpleInterceptingSynchronousResultMethodsWithNoDelay
        : WhenSimpleInterceptingSynchronousResultMethodsBase
    {
        public WhenSimpleInterceptingSynchronousResultMethodsWithNoDelay() : base(0)
        {
        }
    }

    public class WhenSimpleInterceptingSynchronousResultMethodsWithADelay
        : WhenSimpleInterceptingSynchronousResultMethodsBase
    {
        public WhenSimpleInterceptingSynchronousResultMethodsWithADelay() : base(10)
        {
        }
    }

    public abstract class WhenSimpleInterceptingAsynchronousVoidMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        protected WhenSimpleInterceptingAsynchronousVoidMethodsBase(int msDelay)
        {
            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestSimpleAsyncInterceptor(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
        }

        [Fact]
        public async Task ShouldLog4Entries()
        {
            // Act
            await _proxy.AsynchronousVoidMethod();

            // Assert
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public async Task ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:StartingVoidInvocation", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowProcessingAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedVoidInvocation", _log[3]);
        }
    }

    public class WhenSimpleInterceptingAsynchronousVoidMethodsWithNoDelay
        : WhenSimpleInterceptingAsynchronousVoidMethodsBase
    {
        public WhenSimpleInterceptingAsynchronousVoidMethodsWithNoDelay() : base(0)
        {
        }
    }

    public class WhenSimpleInterceptingAsynchronousVoidMethodsWithADelay
        : WhenSimpleInterceptingAsynchronousVoidMethodsBase
    {
        public WhenSimpleInterceptingAsynchronousVoidMethodsWithADelay() : base(10)
        {
        }
    }

    public abstract class WhenSimpleInterceptingAsynchronousResultMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
        private readonly List<string> _log = new List<string>();
        private readonly IInterfaceToProxy _proxy;

        protected WhenSimpleInterceptingAsynchronousResultMethodsBase(int msDelay)
        {
            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestSimpleAsyncInterceptor(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
        }

        [Fact]
        public async Task ShouldLog4Entries()
        {
            // Act
            Guid result = await _proxy.AsynchronousResultMethod();

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public async Task ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:StartingResultInvocation", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowProcessingAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedResultInvocation", _log[3]);
        }
    }

    public class WhenSimpleInterceptingAsynchronousResultMethodsWithNoDelay
        : WhenSimpleInterceptingAsynchronousResultMethodsBase
    {
        public WhenSimpleInterceptingAsynchronousResultMethodsWithNoDelay() : base(0)
        {
        }
    }

    public class WhenSimpleInterceptingAsynchronousResultMethodsWithADelay
        : WhenSimpleInterceptingAsynchronousResultMethodsBase
    {
        public WhenSimpleInterceptingAsynchronousResultMethodsWithADelay() : base(10)
        {
        }
    }
}
