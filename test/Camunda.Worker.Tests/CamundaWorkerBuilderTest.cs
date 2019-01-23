// Copyright (c) Alexey Malinin. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Camunda.Worker.Execution;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Camunda.Worker
{
    public class CamundaWorkerBuilderTest
    {
        [Fact]
        public void TestAdd()
        {
            var services = new ServiceCollection();
            var handlerMock = new Mock<IExternalTaskHandler>();

            var builder = new CamundaWorkerBuilder(services);

            builder.AddHandlerDescriptor(new HandlerDescriptor("testTopic", provider => handlerMock.Object));

            Assert.Contains(services, d => d.Lifetime == ServiceLifetime.Singleton &&
                                           d.ImplementationInstance != null);
        }

        [Fact]
        public void TestAddNull()
        {
            var services = new ServiceCollection();
            var builder = new CamundaWorkerBuilder(services);

            Assert.Throws<ArgumentNullException>(() => builder.AddHandlerDescriptor(null));
        }

        [Fact]
        public void TestAddFactoryProvider()
        {
            var services = new ServiceCollection();
            var builder = new CamundaWorkerBuilder(services);

            builder.AddFactoryProvider<HandlerFactoryProvider>();

            Assert.Contains(services, d => d.Lifetime == ServiceLifetime.Transient &&
                                           d.ServiceType == typeof(IHandlerFactoryProvider) &&
                                           d.ImplementationType == typeof(HandlerFactoryProvider));
        }

        private class HandlerFactoryProvider : IHandlerFactoryProvider
        {
            public HandlerFactory GetHandlerFactory(ExternalTask externalTask)
            {
                throw new NotImplementedException();
            }
        }
    }
}
