// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;

    /// <summary>
    /// Interface to handle synchronous action
    /// </summary>
    public interface IActionInvocation : IAsyncInvocation
    {
        /// <summary>
        ///   Proceeds the call to the next interceptor in line, and ultimately to the target method.
        /// </summary>
        /// <remarks>
        ///   Since interface proxies without a target don't have the target implementation to proceed to,
        ///   it is important, that the last interceptor does not call this method, otherwise a
        ///   <see cref = "NotImplementedException" /> will be thrown.
        /// </remarks>
        void Proceed();
    }
}
