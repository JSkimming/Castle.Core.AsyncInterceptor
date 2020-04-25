// Copyright (c) 2016-2020 James Skimming. All rights reserved.
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

        protected WhenInterceptingSynchronousVoidMethodsBase(
            ITestOutputHelper output,
            bool asyncB4Proceed,
            int msDelayAfterProceed)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work by the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed, msDelayAfterProceed);
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

    public class WhenInterceptingSynchronousVoidMethodsWithAsyncB4AndNoDelay
        : WhenInterceptingSynchronousVoidMethodsBase
    {
        public WhenInterceptingSynchronousVoidMethodsWithAsyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenInterceptingSynchronousVoidMethodsWithSyncB4AndNoDelay
        : WhenInterceptingSynchronousVoidMethodsBase
    {
        public WhenInterceptingSynchronousVoidMethodsWithSyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenInterceptingSynchronousVoidMethodsWithAsyncB4AndADelay
        : WhenInterceptingSynchronousVoidMethodsBase
    {
        public WhenInterceptingSynchronousVoidMethodsWithAsyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenInterceptingSynchronousVoidMethodsWithSyncB4AndADelay
        : WhenInterceptingSynchronousVoidMethodsBase
    {
        public WhenInterceptingSynchronousVoidMethodsWithSyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 10)
        {
        }
    }

    public abstract class WhenInterceptingSynchronousResultMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        protected WhenInterceptingSynchronousResultMethodsBase(
            ITestOutputHelper output,
            bool asyncB4Proceed,
            int msDelayAfterProceed)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed, msDelayAfterProceed);
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

    public class WhenInterceptingSynchronousResultMethodsWithAsyncB4AndNoDelay
        : WhenInterceptingSynchronousResultMethodsBase
    {
        public WhenInterceptingSynchronousResultMethodsWithAsyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenInterceptingSynchronousResultMethodsWithSyncB4AndNoDelay
        : WhenInterceptingSynchronousResultMethodsBase
    {
        public WhenInterceptingSynchronousResultMethodsWithSyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenInterceptingSynchronousResultMethodsWithAsyncB4AndADelay
        : WhenInterceptingSynchronousResultMethodsBase
    {
        public WhenInterceptingSynchronousResultMethodsWithAsyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenInterceptingSynchronousResultMethodsWithSyncB4AndADelay
        : WhenInterceptingSynchronousResultMethodsBase
    {
        public WhenInterceptingSynchronousResultMethodsWithSyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 10)
        {
        }
    }

    public abstract class WhenInterceptingAsynchronousVoidMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        protected WhenInterceptingAsynchronousVoidMethodsBase(
            ITestOutputHelper output,
            bool asyncB4Proceed,
            int msDelayAfterProceed)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed, msDelayAfterProceed);
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

    public class WhenInterceptingAsynchronousVoidMethodsWithAsyncB4AndNoDelay
        : WhenInterceptingAsynchronousVoidMethodsBase
    {
        public WhenInterceptingAsynchronousVoidMethodsWithAsyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenInterceptingAsynchronousVoidMethodsWithSyncB4AndNoDelay
        : WhenInterceptingAsynchronousVoidMethodsBase
    {
        public WhenInterceptingAsynchronousVoidMethodsWithSyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenInterceptingAsynchronousVoidMethodsWithAsyncB4AndADelay
        : WhenInterceptingAsynchronousVoidMethodsBase
    {
        public WhenInterceptingAsynchronousVoidMethodsWithAsyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenInterceptingAsynchronousVoidMethodsWithSyncB4AndADelay
        : WhenInterceptingAsynchronousVoidMethodsBase
    {
        public WhenInterceptingAsynchronousVoidMethodsWithSyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 10)
        {
        }
    }

    public abstract class WhenInterceptingAsynchronousResultMethodsBase
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
        private readonly ListLogger _log;
        private readonly IInterfaceToProxy _proxy;

        protected WhenInterceptingAsynchronousResultMethodsBase(
            ITestOutputHelper output,
            bool asyncB4Proceed,
            int msDelayAfterProceed)
        {
            _log = new ListLogger(output);

            // The delay is used to simulate work my the interceptor, thereof not always continuing on the same thread.
            var interceptor = new TestAsyncInterceptorBase(_log, asyncB4Proceed, msDelayAfterProceed);
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

    public class WhenInterceptingAsynchronousResultMethodsWithAsyncB4AndNoDelay
        : WhenInterceptingAsynchronousResultMethodsBase
    {
        public WhenInterceptingAsynchronousResultMethodsWithAsyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenInterceptingAsynchronousResultMethodsWithSyncB4AndNoDelay
        : WhenInterceptingAsynchronousResultMethodsBase
    {
        public WhenInterceptingAsynchronousResultMethodsWithSyncB4AndNoDelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 0)
        {
        }
    }

    public class WhenInterceptingAsynchronousResultMethodsWithAsyncB4AndADelay
        : WhenInterceptingAsynchronousResultMethodsBase
    {
        public WhenInterceptingAsynchronousResultMethodsWithAsyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: true, msDelayAfterProceed: 10)
        {
        }
    }

    public class WhenInterceptingAsynchronousResultMethodsWithSyncB4AndADelay
        : WhenInterceptingAsynchronousResultMethodsBase
    {
        public WhenInterceptingAsynchronousResultMethodsWithSyncB4AndADelay(ITestOutputHelper output)
            : base(output, asyncB4Proceed: false, msDelayAfterProceed: 10)
        {
        }
    }
}
