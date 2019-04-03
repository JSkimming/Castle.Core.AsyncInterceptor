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

    public class WhenProcessingSynchronousVoidMethodsWithTheReturnValue
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousVoidMethod);
        private readonly ListLogger _log = new ListLogger();
        private readonly TestProcessingReturnValueAsyncInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenProcessingSynchronousVoidMethodsWithTheReturnValue()
        {
            _interceptor = new TestProcessingReturnValueAsyncInterceptor(_log);
            _proxy = ProxyGen.CreateProxy(_log, _interceptor);
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
            Assert.Equal($"{MethodName}:StartingInvocation", _log[0]);
        }

        [Fact]
        public void ShouldAllowProcessingAfterInvocation()
        {
            // Act
            _proxy.SynchronousVoidMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedInvocation:(no return value)", _log[3]);
        }
    }

    public class WhenProcessingSynchronousResultMethodsWithTheReturnValue
    {
        private const string MethodName = nameof(IInterfaceToProxy.SynchronousResultMethod);
        private readonly ListLogger _log = new ListLogger();
        private readonly TestProcessingReturnValueAsyncInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenProcessingSynchronousResultMethodsWithTheReturnValue()
        {
            _interceptor = new TestProcessingReturnValueAsyncInterceptor(_log);
            _proxy = ProxyGen.CreateProxy(_log, _interceptor);
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
            Assert.Equal($"{MethodName}:StartingInvocation", _log[0]);
        }

        [Fact]
        public void ShouldAllowProcessingAfterInvocation()
        {
            // Act
            Guid returnValue = _proxy.SynchronousResultMethod();

            // Assert
            Assert.Equal($"{MethodName}:CompletedInvocation:{returnValue}", _log[3]);
        }
    }

    public class WhenProcessingAsynchronousVoidMethodsWithTheReturnValue
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousVoidMethod);
        private readonly ListLogger _log = new ListLogger();
        private readonly TestProcessingReturnValueAsyncInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenProcessingAsynchronousVoidMethodsWithTheReturnValue()
        {
            _interceptor = new TestProcessingReturnValueAsyncInterceptor(_log);
            _proxy = ProxyGen.CreateProxy(_log, _interceptor);
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
            Assert.Equal($"{MethodName}:StartingInvocation", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowProcessingAfterInvocation()
        {
            // Act
            await _proxy.AsynchronousVoidMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:CompletedInvocation:(no return value)", _log[3]);
        }
    }

    public class WhenProcessingAsynchronousResultMethodsWithTheReturnValue
    {
        private const string MethodName = nameof(IInterfaceToProxy.AsynchronousResultMethod);
        private readonly ListLogger _log = new ListLogger();
        private readonly TestProcessingReturnValueAsyncInterceptor _interceptor;
        private readonly IInterfaceToProxy _proxy;

        public WhenProcessingAsynchronousResultMethodsWithTheReturnValue()
        {
            _interceptor = new TestProcessingReturnValueAsyncInterceptor(_log);
            _proxy = ProxyGen.CreateProxy(_log, _interceptor);
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
            Assert.Equal($"{MethodName}:StartingInvocation", _log[0]);
        }

        [Fact]
        public async Task ShouldAllowProcessingAfterInvocation()
        {
            // Act
            Guid returnValue = await _proxy.AsynchronousResultMethod().ConfigureAwait(false);

            // Assert
            Assert.Equal($"{MethodName}:CompletedInvocation:{returnValue}", _log[3]);
        }
    }
}
