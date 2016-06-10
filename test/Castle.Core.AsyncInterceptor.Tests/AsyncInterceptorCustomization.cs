// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class AsyncInterceptorCustomization : CompositeCustomization
    {
        public AsyncInterceptorCustomization()
            : base(new AutoMoqCustomization())
        {
        }
    }
}
