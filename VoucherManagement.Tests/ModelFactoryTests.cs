using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.Tests
{
    using Factories;
    using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
    using Models;
    using Shouldly;
    using Testing;
    using Xunit;

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
    }
}
