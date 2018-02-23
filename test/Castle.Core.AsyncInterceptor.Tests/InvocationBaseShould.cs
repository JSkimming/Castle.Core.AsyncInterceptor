// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace Castle.DynamicProxy
{
    using System;
    using System.Reflection;

    using Castle.DynamicProxy.Invocations;

    using Moq;

    using Xunit;

    public class InvocationBaseShould
    {
        private InvocationBase _invocationBase;

        private Mock<IInvocation> _invocationMock;

        public InvocationBaseShould()
        {
            _invocationMock = new Mock<IInvocation>();
            _invocationBase = new ActionInvocation(_invocationMock.Object);
        }

        [Fact]
        public void ShouldReturnArguments()
        {
            // Act
            object[] _ = _invocationBase.Arguments;

            // Assert
            _invocationMock.VerifyGet(i => i.Arguments);
        }

        [Fact]
        public void ShouldReturnGenericArguments()
        {
            // Act
            Type[] _ = _invocationBase.GenericArguments;

            // Assert
            _invocationMock.VerifyGet(i => i.GenericArguments);
        }

        [Fact]
        public void ShouldReturnInvocationTarget()
        {
            // Act
            object _ = _invocationBase.InvocationTarget;

            // Assert
            _invocationMock.VerifyGet(i => i.InvocationTarget);
        }

        [Fact]
        public void ShouldReturnMethod()
        {
            // Act
            MethodInfo _ = _invocationBase.Method;

            // Assert
            _invocationMock.VerifyGet(i => i.Method);
        }

        [Fact]
        public void ShouldReturnMethodInvocationTarget()
        {
            // Act
            MethodInfo _ = _invocationBase.MethodInvocationTarget;

            // Assert
            _invocationMock.VerifyGet(i => i.MethodInvocationTarget);
        }

        [Fact]
        public void ShouldReturnProxy()
        {
            // Act
            object _ = _invocationBase.Proxy;

            // Assert
            _invocationMock.VerifyGet(i => i.Proxy);
        }

        [Fact]
        public void ShouldReturnTargetType()
        {
            // Act
            object _ = _invocationBase.TargetType;

            // Assert
            _invocationMock.VerifyGet(i => i.TargetType);
        }

        [Fact]
        public void ShouldReturnGetArgumentValue()
        {
            // Act
            object _ = _invocationBase.GetArgumentValue(4711);

            // Assert
            _invocationMock.Verify(i => i.GetArgumentValue(4711));
        }

        [Fact]
        public void ShouldReturnGetConcreteMethod()
        {
            // Act
            MethodInfo _ = _invocationBase.GetConcreteMethod();

            // Assert
            _invocationMock.Verify(i => i.GetConcreteMethod());
        }

        [Fact]
        public void ShouldReturnGetConcreteMethodInvocationTarget()
        {
            // Act
            MethodInfo _ = _invocationBase.GetConcreteMethodInvocationTarget();

            // Assert
            _invocationMock.Verify(i => i.GetConcreteMethodInvocationTarget());
        }

        [Fact]
        public void ShouldReturnSetArgumentValue()
        {
            // Act
            object anObject = new object();
            _invocationBase.SetArgumentValue(4712, anObject);

            // Assert
            _invocationMock.Verify(i => i.SetArgumentValue(4712, anObject));
        }
    }
}
