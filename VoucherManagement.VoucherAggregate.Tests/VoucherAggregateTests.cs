using System;
using Xunit;

namespace VoucherManagment.VoucherAggregate.Tests
{
    using System.IO;
    using Shouldly;
    using VoucherManagement.Testing;
    using VoucherManagement.VoucherAggregate;

    public class VoucherAggregateTests
    {
        [Fact]
        public void VoucherAggregate_CanBeCreated_IsCreated()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);

            aggregate.AggregateId.ShouldBe(TestData.VoucherId);
        }

        [Fact]
        public void VoucherAggregate_Generate_VoucherIsGenerated()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.Value);

            aggregate.IsGenerated.ShouldBeTrue();
            aggregate.EstateId.ShouldBe(TestData.EstateId);
            aggregate.IsIssued.ShouldBeFalse();
            aggregate.VoucherCode.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public void VoucherAggregate_Generate_VoucherAlreadyGenerated_ErrorThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.Value);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.Value);
                                                    });
        }

        [Fact]
        public void VoucherAggregate_Generate_VoucherAlreadyIssued_ErrorThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.Value);
            aggregate.Issue(TestData.RecipientEmail, TestData.RecipientMobile);
            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.Value);
                                                    });
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void VoucherAggregate_Generate_InvalidOperatorIdentifier_ErrorIsThrown(String operatorIdentifier)
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);

            Should.Throw<ArgumentNullException>(() =>
                                                      {
                                                          aggregate.Generate(operatorIdentifier, TestData.EstateId, TestData.Value);
                                                      });
        }

        [Fact]
        public void VoucherAggregate_Generate_InvalidEstateId_ErrorIsThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Generate(TestData.OperatorIdentifier, Guid.Empty, TestData.Value);
                                                });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void VoucherAggregate_Generate_InvalidValue_ErrorIsThrown(Decimal value)
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            
            Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, value);
            });
        }


        [Fact]
        public void VoucherAggregate_Issue_VoucherIsIssued()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.Value);
            aggregate.Issue(TestData.RecipientEmail, TestData.RecipientMobile);

            aggregate.IsIssued.ShouldBeTrue();
        }

        [Fact]
        public void VoucherAggregate_Issue_VoucherNotGenerated_ErrorThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.Issue(TestData.RecipientEmail, TestData.RecipientMobile);
                                                    });
        }
        [Fact]
        public void VoucherAggregate_Issue_VoucherAlreadyIssued_ErrorThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.Value);
            aggregate.Issue(TestData.RecipientEmail, TestData.RecipientMobile);

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.Issue(TestData.RecipientEmail, TestData.RecipientMobile);
            });
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(null, "")]
        public void VoucherAggregate_Issue_EitherEmailOrMobileIsRequired_ErrorThrown(String recipientEmail, String recipientMobile)
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.Value);
            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Issue(recipientEmail,recipientMobile);
                                                });
        }
    }
}
