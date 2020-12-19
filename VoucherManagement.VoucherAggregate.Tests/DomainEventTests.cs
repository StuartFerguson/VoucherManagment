using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.VoucherAggregate.Tests
{
    using Shouldly;
    using VoucherManagement.Testing;
    using VoucherManagement.Voucher.DomainEvents;
    using Xunit;

    public class DomainEventTests
    {
        [Fact]
        public void VoucherGeneratedEvent_CanBeCreated_IsCreated()
        {
            VoucherGeneratedEvent voucherGeneratedEvent = VoucherGeneratedEvent.Create(TestData.VoucherId,TestData.EstateId,
                                                                                       TestData.TransactionId, TestData.IssuedDateTime,
                                                                                       TestData.OperatorIdentifier,
                                                                                       TestData.Value, TestData.VoucherCode,TestData.ExpiryDate,
                                                                                       TestData.Message);

            voucherGeneratedEvent.ShouldNotBeNull();
            voucherGeneratedEvent.AggregateId.ShouldBe(TestData.VoucherId);
            voucherGeneratedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            voucherGeneratedEvent.EventId.ShouldNotBe(Guid.Empty);
            voucherGeneratedEvent.VoucherId.ShouldBe(TestData.VoucherId);
            voucherGeneratedEvent.EstateId.ShouldBe(TestData.EstateId);
            voucherGeneratedEvent.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier);
            voucherGeneratedEvent.Value.ShouldBe(TestData.Value);
            voucherGeneratedEvent.VoucherCode.ShouldBe(TestData.VoucherCode);
            voucherGeneratedEvent.ExpiryDateTime.ShouldBe(TestData.ExpiryDate);
            voucherGeneratedEvent.Message.ShouldBe(TestData.Message);
            voucherGeneratedEvent.GeneratedDateTime.ShouldBe(TestData.IssuedDateTime);
        }

        [Fact]
        public void VoucherIssuedEvent_CanBeCreated_IsCreated()
        {
            VoucherIssuedEvent voucherIssuedEvent = VoucherIssuedEvent.Create(TestData.VoucherId, TestData.EstateId, TestData.IssuedDateTime, TestData.RecipientEmail,TestData.RecipientMobile);

            voucherIssuedEvent.ShouldNotBeNull();
            voucherIssuedEvent.AggregateId.ShouldBe(TestData.VoucherId);
            voucherIssuedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            voucherIssuedEvent.EventId.ShouldNotBe(Guid.Empty);
            voucherIssuedEvent.VoucherId.ShouldBe(TestData.VoucherId);
            voucherIssuedEvent.EstateId.ShouldBe(TestData.EstateId);
            voucherIssuedEvent.RecipientEmail.ShouldBe(TestData.RecipientEmail);
            voucherIssuedEvent.RecipientMobile.ShouldBe(TestData.RecipientMobile);
            voucherIssuedEvent.IssuedDateTime.ShouldBe(TestData.IssuedDateTime);
        }
    }
}
