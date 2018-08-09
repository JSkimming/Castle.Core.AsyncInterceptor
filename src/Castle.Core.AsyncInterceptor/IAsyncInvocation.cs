// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Maps Castle.Core IInvocation. Except ReturnValue and Proceed method
    /// </summary>
    public interface IAsyncInvocation
    {
        /// <summary>
        ///   Gets the arguments that the <see cref = "Method" /> has been invoked with.
        /// </summary>
        /// <value>The arguments the method was invoked with.</value>
        object[] Arguments { get; }

        /// <summary>
        ///   Gets the generic arguments of the method.
        /// </summary>
        /// <value>The generic arguments, or null if not a generic method.</value>
        Type[] GenericArguments { get; }

        /// <summary>
        ///   Gets the object on which the invocation is performed. This is different from proxy object
        ///   because most of the time this will be the proxy target object.
        /// </summary>
        /// <seealso cref = "IChangeProxyTarget" />
        /// <value>The invocation target.</value>
        object InvocationTarget { get; }

        /// <summary>
        ///   Gets the <see cref = "MethodInfo" /> representing the method being invoked on the proxy.
        /// </summary>
        /// <value>The <see cref = "MethodInfo" /> representing the method being invoked.</value>
        MethodInfo Method { get; }

        /// <summary>
        /// Gets - For interface proxies, this will point to the <see cref = "MethodInfo" /> on the target class.
        /// </summary>
        /// <value>The method invocation target.</value>
        MethodInfo MethodInvocationTarget { get; }

        /// <summary>
        ///   Gets the proxy object on which the intercepted method is invoked.
        /// </summary>
        /// <value>Proxy object on which the intercepted method is invoked.</value>
        object Proxy { get; }

        /// <summary>
        ///   Gets the type of the target object for the intercepted method.
        /// </summary>
        /// <value>The type of the target object.</value>
        Type TargetType { get; }

        /// <summary>
        ///   Gets the value of the argument at the specified <paramref name = "index" />.
        /// </summary>
        /// <param name = "index">The index.</param>
        /// <returns>The value of the argument at the specified <paramref name = "index" />.</returns>
        object GetArgumentValue(int index);

        /// <summary>
        ///   Returns the concrete instantiation of the <see cref = "Method" /> on the proxy, with any generic
        ///   parameters bound to real types.
        /// </summary>
        /// <returns>
        ///   The concrete instantiation of the <see cref = "Method" /> on the proxy, or the <see cref = "Method" /> if
        ///   not a generic method.
        /// </returns>
        /// <remarks>
        ///   Can be slower than calling <see cref = "Method" />.
        /// </remarks>
        MethodInfo GetConcreteMethod();

        /// <summary>
        ///   Returns the concrete instantiation of <see cref = "MethodInvocationTarget" />, with any
        ///   generic parameters bound to real types.
        ///   For interface proxies, this will point to the <see cref = "MethodInfo" /> on the target class.
        /// </summary>
        /// <returns>The concrete instantiation of <see cref = "MethodInvocationTarget" />, or
        ///   <see cref = "MethodInvocationTarget" /> if not a generic method.</returns>
        /// <remarks>
        ///   In debug builds this can be slower than calling <see cref = "MethodInvocationTarget" />.
        /// </remarks>
        MethodInfo GetConcreteMethodInvocationTarget();

        /// <summary>
        ///   Overrides the value of an argument at the given <paramref name = "index" /> with the
        ///   new <paramref name = "value" /> provided.
        /// </summary>
        /// <remarks>
        ///   This method accepts an <see cref = "object" />, however the value provided must be compatible
        ///   with the type of the argument defined on the method, otherwise an exception will be thrown.
        /// </remarks>
        /// <param name = "index">The index of the argument to override.</param>
        /// <param name = "value">The new value for the argument.</param>
        void SetArgumentValue(int index, object value);
    }
}
