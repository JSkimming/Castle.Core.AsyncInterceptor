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
    using Xunit.Abstractions;

    public abstract class WhenInterceptingSynchronousVoidMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        protected WhenInterceptingSynchronousVoidMethodsBase(ITestOutputHelper output, int msDelay)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work by the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, msDelay);
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

    public class WhenInterceptingSynchronousVoidMethodsWithNoDelay
        : WhenInterceptingSynchronousVoidMethodsBase
    {
        public WhenInterceptingSynchronousVoidMethodsWithNoDelay(ITestOutputHelper output) : base(output, 0)
        {
        }
    }

    public class WhenInterceptingSynchronousVoidMethodsWithADelay
        : WhenInterceptingSynchronousVoidMethodsBase
    {
        public WhenInterceptingSynchronousVoidMethodsWithADelay(ITestOutputHelper output) : base(output, 10)
        {
        }
    }

    public abstract class WhenInterceptingSynchronousResultMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        protected WhenInterceptingSynchronousResultMethodsBase(ITestOutputHelper output, int msDelay)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, msDelay);
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
            Assert.Equal($"{MethodName}:StartingResultInvocation", _log[0]);
        }

        [Fact]
        public void ShouldAllowProcessingAfterInvocation()
        {
            // Act
            _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedResultInvocation", _log[3]);
        }
    }

    public class WhenInterceptingSynchronousResultMethodsWithNoDelay
        : WhenInterceptingSynchronousResultMethodsBase
    {
        public WhenInterceptingSynchronousResultMethodsWithNoDelay(ITestOutputHelper output) : base(output, 0)
        {
        }
    }

    public class WhenInterceptingSynchronousResultMethodsWithADelay
        : WhenInterceptingSynchronousResultMethodsBase
    {
        public WhenInterceptingSynchronousResultMethodsWithADelay(ITestOutputHelper output) : base(output, 10)
        {
        }
    }

    public abstract class WhenInterceptingAsynchronousVoidMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        protected WhenInterceptingAsynchronousVoidMethodsBase(ITestOutputHelper output, int msDelay)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
        }

        [Fact]
        public async Task ShouldLog4Entries()
        {
            // Act
            await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public async Task ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:StartingVoidInvocation", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowProcessingAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:CompletedVoidInvocation", _log[3]);
        }
    }

    public class WhenInterceptingAsynchronousVoidMethodsWithNoDelay
        : WhenInterceptingAsynchronousVoidMethodsBase
    {
        public WhenInterceptingAsynchronousVoidMethodsWithNoDelay(ITestOutputHelper output) : base(output, 0)
        {
        }
    }

    public class WhenInterceptingAsynchronousVoidMethodsWithADelay
        : WhenInterceptingAsynchronousVoidMethodsBase
    {
        public WhenInterceptingAsynchronousVoidMethodsWithADelay(ITestOutputHelper output) : base(output, 10)
        {
        }
    }

    public abstract class WhenInterceptingAsynchronousResultMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        protected WhenInterceptingAsynchronousResultMethodsBase(ITestOutputHelper output, int msDelay)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, msDelay);
            _proxy = ProxyGen.CreateProxy(_log, interceptor);
        }

        [Fact]
        public async Task ShouldLog4Entries()
        {
            // Act
            Guid result = await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(4, _log.Count);
        }

        [Fact]
        public async Task ShouldAllowProcessingPriorToInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:StartingResultInvocation", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowProcessingAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:CompletedResultInvocation", _log[3]);
        }
    }

    public class WhenInterceptingAsynchronousResultMethodsWithNoDelay
        : WhenInterceptingAsynchronousResultMethodsBase
    {
        public WhenInterceptingAsynchronousResultMethodsWithNoDelay(ITestOutputHelper output) : base(output, 0)
        {
        }
    }

    public class WhenInterceptingAsynchronousResultMethodsWithADelay
        : WhenInterceptingAsynchronousResultMethodsBase
    {
        public WhenInterceptingAsynchronousResultMethodsWithADelay(ITestOutputHelper output) : base(output, 10)
        {
        }
    }
}
