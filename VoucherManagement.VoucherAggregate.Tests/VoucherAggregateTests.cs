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
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);

            var voucher = aggregate.GetVoucher();

            voucher.IsGenerated.ShouldBeTrue();
            voucher.EstateId.ShouldBe(TestData.EstateId);
            voucher.IsIssued.ShouldBeFalse();
            voucher.IssuedDateTime.ShouldBe(TestData.IssuedDateTime);
            voucher.VoucherCode.ShouldNotBeNullOrEmpty();
            voucher.TransactionId.ShouldBe(TestData.TransactionId);
        }

        [Fact]
        public void VoucherAggregate_Generate_VoucherAlreadyGenerated_ErrorThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
                                                    });
        }

        [Fact]
        public void VoucherAggregate_Generate_VoucherAlreadyIssued_ErrorThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
            aggregate.Issue(TestData.RecipientEmail, TestData.RecipientMobile);
            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
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
                                                          aggregate.Generate(operatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
                                                      });
        }

        [Fact]
        public void VoucherAggregate_Generate_InvalidEstateId_ErrorIsThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Generate(TestData.OperatorIdentifier, Guid.Empty, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
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
                aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, value);
            });
        }


        [Fact]
        public void VoucherAggregate_Issue_VoucherIsIssued()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
            aggregate.Issue(TestData.RecipientEmail, TestData.RecipientMobile);
            var voucher = aggregate.GetVoucher();
            voucher.IsIssued.ShouldBeTrue();
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
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
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
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    aggregate.Issue(recipientEmail,recipientMobile);
                                                });
        }

        [Fact]
        public void VoucherAggregate_AddBarcode_BarcodeIsAdded()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
            aggregate.AddBarcode(TestData.Barcode);
            var voucher = aggregate.GetVoucher();
            voucher.Barcode.ShouldBe(TestData.Barcode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void VoucherAggregate_AddBarcode_InvalidBarcode_ErrorThrown(String barcode)
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);

            Should.Throw<ArgumentException>(() =>
            {
                aggregate.AddBarcode(barcode);
            });
        }

        [Fact]
        public void VoucherAggregate_AddBarcode_VoucherNotGenerated_ErrorThrown()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            
            Should.Throw<InvalidOperationException>(() =>
                                            {
                                                aggregate.AddBarcode(TestData.Barcode);
                                            });
        }
    }
}
