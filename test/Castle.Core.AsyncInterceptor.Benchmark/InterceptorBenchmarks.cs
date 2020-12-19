// Copyright (c) 2016-2020 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;

    public class InterceptorBenchmarks
    {
        private static readonly IProxyGenerator Generator = new ProxyGenerator();

        private static readonly ITarget RawTarget = new Target();

        private static readonly ITarget NopInterceptorTarget =
            Generator.CreateInterfaceProxyWithTargetInterface(RawTarget, new NopInterceptor());

        private static readonly ITarget NopAsyncInterceptorTarget =
            Generator.CreateInterfaceProxyWithTargetInterface(RawTarget, new NopAsyncInterceptor());

        private ITarget _target = RawTarget;
        private InterceptorType _type;

        public enum InterceptorType
        {
            /// <summary>
            /// No interceptor.
            /// </summary>
            NoInterceptor,

            /// <summary>
            /// No operation interceptor.
            /// </summary>
            NopInterceptor,

            /// <summary>
            /// No operation async interceptor.
            /// </summary>
            NopAsyncInterceptor,
        }

        [Params(
            InterceptorType.NoInterceptor,
            InterceptorType.NopInterceptor,
            InterceptorType.NopAsyncInterceptor)]
        public InterceptorType Type
        {
            get => _type;
            set
            {
                _type = value;
                SetTarget(_type);
            }
        }

        [Benchmark]
        public void VoidSynchronous() => _target.VoidSynchronous();

        [Benchmark]
        public void ResultSynchronous() => _target.ResultSynchronous();

        [Benchmark]
        public Task CompletedTaskAsynchronous() => _target.CompletedTaskAsynchronous();

        [Benchmark]
        public Task CompletedResultTaskAsynchronous() => _target.CompletedResultTaskAsynchronous();

        [Benchmark]
        public Task IncompleteTaskAsynchronous() => _target.IncompleteTaskAsynchronous();

        [Benchmark]
        public Task IncompleteResultTaskAsynchronous() => _target.IncompleteResultTaskAsynchronous();

        private void SetTarget(InterceptorType interceptorType)
        {
            _target = interceptorType switch
            {
                InterceptorType.NopInterceptor => NopInterceptorTarget,
                InterceptorType.NopAsyncInterceptor => NopAsyncInterceptorTarget,
                _ => RawTarget,
            };
        }
    }
}
