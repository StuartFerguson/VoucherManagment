using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.BusinessLogic.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using EventHandling;
    using Moq;
    using Shared.EventStore.EventHandling;
    using Shouldly;
    using Testing;
    using Voucher.DomainEvents;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DomainEventHandlerResolverTests
    {
        [Fact]
        public void DomainEventHandlerResolver_CanBeCreated_IsCreated()
        {
            Dictionary<String, String[]> eventHandlerConfiguration = new Dictionary<String, String[]>();

            eventHandlerConfiguration.Add("TestEventType1", new String[] { "VoucherManagement.BusinessLogic.EventHandling.VoucherDomainEventHandler, VoucherManagement.BusinessLogic" });

            Mock<IDomainEventHandler> domainEventHandler = new Mock<IDomainEventHandler>();
            Func<Type, IDomainEventHandler> createDomainEventHandlerFunc = (type) => { return domainEventHandler.Object; };
            DomainEventHandlerResolver resolver = new DomainEventHandlerResolver(eventHandlerConfiguration, createDomainEventHandlerFunc);

            resolver.ShouldNotBeNull();
        }

        [Fact]
        public void DomainEventHandlerResolver_CanBeCreated_InvalidEventHandlerType_ErrorThrown()
        {
            Dictionary<String, String[]> eventHandlerConfiguration = new Dictionary<String, String[]>();

            eventHandlerConfiguration.Add("TestEventType1", new String[] { "VoucherManagement.BusinessLogic.EventHandling.NonExistantDomainEventHandler, VoucherManagement.BusinessLogic" });

            Mock<IDomainEventHandler> domainEventHandler = new Mock<IDomainEventHandler>();
            Func<Type, IDomainEventHandler> createDomainEventHandlerFunc = (type) => { return domainEventHandler.Object; };

            Should.Throw<NotSupportedException>(() => new DomainEventHandlerResolver(eventHandlerConfiguration, createDomainEventHandlerFunc));
        }

        [Fact]
        public void DomainEventHandlerResolver_GetDomainEventHandlers_TransactionHasBeenCompletedEvent_EventHandlersReturned()
        {
            String handlerTypeName = "VoucherManagement.BusinessLogic.EventHandling.VoucherDomainEventHandler, VoucherManagement.BusinessLogic";
            Dictionary<String, String[]> eventHandlerConfiguration = new Dictionary<String, String[]>();

            VoucherIssuedEvent voucherIssuedEvent = TestData.VoucherIssuedEvent;

            eventHandlerConfiguration.Add(voucherIssuedEvent.GetType().Name, new String[] { handlerTypeName });

            Mock<IDomainEventHandler> domainEventHandler = new Mock<IDomainEventHandler>();
            Func<Type, IDomainEventHandler> createDomainEventHandlerFunc = (type) => { return domainEventHandler.Object; };

            DomainEventHandlerResolver resolver = new DomainEventHandlerResolver(eventHandlerConfiguration, createDomainEventHandlerFunc);

            List<IDomainEventHandler> handlers = resolver.GetDomainEventHandlers(voucherIssuedEvent);

            handlers.ShouldNotBeNull();
            handlers.Any().ShouldBeTrue();
            handlers.Count.ShouldBe(1);
        }
    }
}
