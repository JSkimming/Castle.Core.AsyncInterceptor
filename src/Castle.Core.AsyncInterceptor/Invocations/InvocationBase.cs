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
        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationBase"/> class.
        /// </summary>
        /// <param name="invocation">
        /// The invocation.
        /// </param>
        protected InvocationBase(IInvocation invocation)
        {
            Invocation = invocation;
        }

        /// <inheritdoc />
        public object[] Arguments => Invocation.Arguments;

        /// <inheritdoc />
        public Type[] GenericArguments => Invocation.GenericArguments;

        /// <inheritdoc />
        public object InvocationTarget => Invocation.InvocationTarget;

        /// <inheritdoc />
        public MethodInfo Method => Invocation.Method;

        /// <inheritdoc />
        public MethodInfo MethodInvocationTarget => Invocation.MethodInvocationTarget;

        /// <inheritdoc />
        public object Proxy => Invocation.Proxy;

        /// <inheritdoc />
        public Type TargetType => Invocation.TargetType;

        /// <summary>
        /// Gets the Castle's invocation
        /// </summary>
        protected IInvocation Invocation { get; }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetArgumentValue(int index) => Invocation.GetArgumentValue(index);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MethodInfo GetConcreteMethod() => Invocation.GetConcreteMethod();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MethodInfo GetConcreteMethodInvocationTarget() => Invocation.GetConcreteMethodInvocationTarget();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetArgumentValue(int index, object value) => Invocation.SetArgumentValue(index, value);
    }
}
