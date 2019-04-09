// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy.Invocations
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <inheritdoc />
    internal abstract class InvocationBase : IAsyncInvocation
    {
        private readonly IInvocationProceedInfo _proceedInfo;
        private readonly IInvocation _invocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationBase"/> class.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        protected InvocationBase(IInvocation invocation)
        {
            _invocation = invocation;
            _proceedInfo = invocation.CaptureProceedInfo();
        }

        /// <inheritdoc />
        public object[] Arguments => _invocation.Arguments;

        /// <inheritdoc />
        public Type[] GenericArguments => _invocation.GenericArguments;

        /// <inheritdoc />
        public object InvocationTarget => _invocation.InvocationTarget;

        /// <inheritdoc />
        public MethodInfo Method => _invocation.Method;

        /// <inheritdoc />
        public MethodInfo MethodInvocationTarget => _invocation.MethodInvocationTarget;

        /// <inheritdoc />
        public object Proxy => _invocation.Proxy;

        /// <inheritdoc />
        public Type TargetType => _invocation.TargetType;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetArgumentValue(int index) => _invocation.GetArgumentValue(index);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MethodInfo GetConcreteMethod() => _invocation.GetConcreteMethod();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MethodInfo GetConcreteMethodInvocationTarget() => _invocation.GetConcreteMethodInvocationTarget();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetArgumentValue(int index, object value) => _invocation.SetArgumentValue(index, value);

        /// <summary>
        /// Invokes proceed.
        /// </summary>
        protected object InvokeProceed()
        {
            _proceedInfo.Invoke();
            return _invocation.ReturnValue;
        }
    }
}
