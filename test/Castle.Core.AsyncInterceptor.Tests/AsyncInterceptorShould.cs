// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ploeh.AutoFixture;
    using Xunit;

    public class AsyncInterceptorShould
    {
        [Fact]
        public void Implement_IInterceptor()
        {
            var fixture = new Fixture().Customize(new AsyncInterceptorCustomization());
            var sut = fixture.Create<AsyncInterceptor>();

            Assert.IsAssignableFrom<IInterceptor>(sut);
        }
    }
}
