using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using Factories;
    using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
    using Models;
    using Shouldly;
    using Testing;
    using Xunit;
    using IssueVoucherResponse = Models.IssueVoucherResponse;
    using RedeemVoucherResponse = Models.RedeemVoucherResponse;

    [ExcludeFromCodeCoverage]
    public class ModelFactoryTests
    {
        [Fact]
        public void ModelFactory_ConvertFrom_IssueVoucherResponse_IsConverted()
        {
            ModelFactory modelFactory = new ModelFactory();
            IssueVoucherResponse model = TestData.IssueVoucherResponse;
            DataTransferObjects.IssueVoucherResponse dto = modelFactory.ConvertFrom(model);

            dto.ShouldNotBeNull();
            dto.ExpiryDate.ShouldBe(model.ExpiryDate);
            dto.Message.ShouldBe(model.Message);
            dto.VoucherCode.ShouldBe(model.VoucherCode);
            dto.VoucherId.ShouldBe(model.VoucherId);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_IssueVoucherResponse_NullInput_IsConverted()
        {
            ModelFactory modelFactory = new ModelFactory();
            IssueVoucherResponse model = null;
            DataTransferObjects.IssueVoucherResponse dto = modelFactory.ConvertFrom(model);

            dto.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_VoucherModel_IsConverted()
        {
            ModelFactory modelFactory = new ModelFactory();
            Voucher model = TestData.GetVoucherAggregateWithRecipientMobile().GetVoucher();

            GetVoucherResponse dto = modelFactory.ConvertFrom(model);
            dto.ShouldNotBeNull();
            dto.TransactionId.ShouldBe(model.TransactionId);
            dto.IssuedDateTime.ShouldBe(model.IssuedDateTime);
            dto.Balance.ShouldBe(model.Balance);
            dto.IssuedDateTime.ShouldBe(model.IssuedDateTime);
            dto.ExpiryDate.ShouldBe(model.ExpiryDate);
            dto.GeneratedDateTime.ShouldBe(model.GeneratedDateTime);
            dto.IsGenerated.ShouldBe(model.IsGenerated);
            dto.IsIssued.ShouldBe(model.IsIssued);
            dto.IsRedeemed.ShouldBe(model.IsRedeemed);
            dto.RedeemedDateTime.ShouldBe(model.RedeemedDateTime);
            dto.Value.ShouldBe(model.Value);
            dto.VoucherCode.ShouldBe(model.VoucherCode);
            dto.Balance.ShouldBe(model.Balance);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_VoucherModel_NullInput_IsConverted()
        {
            ModelFactory modelFactory = new ModelFactory();
            Voucher model = null;
            GetVoucherResponse dto = modelFactory.ConvertFrom(model);

            dto.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_RedeemVoucherResponse_IsConverted()
        {
            ModelFactory modelFactory = new ModelFactory();
            RedeemVoucherResponse model = TestData.RedeemVoucherResponse;
            DataTransferObjects.RedeemVoucherResponse dto = modelFactory.ConvertFrom(model);

            dto.ShouldNotBeNull();
            dto.ExpiryDate.ShouldBe(model.ExpiryDate);
            dto.VoucherCode.ShouldBe(model.VoucherCode);
            dto.RemainingBalance.ShouldBe(model.RemainingBalance);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_RedeemVoucherResponse_NullInput_IsConverted()
        {
            ModelFactory modelFactory = new ModelFactory();
            IssueVoucherResponse model = null;
            DataTransferObjects.IssueVoucherResponse dto = modelFactory.ConvertFrom(model);

            dto.ShouldBeNull();
        }
    }
}
