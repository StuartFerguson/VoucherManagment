using Xunit;

namespace VoucherManagement.BusinessLogic.Tests
{
    using Models;
    using Requests;
    using Shouldly;
    using Testing;

    public class RequestTests
    {
        [Fact]
        public void IssueVoucherRequest_CanBeCreated_IsCreated()
        {
            IssueVoucherRequest issueVoucherRequest = IssueVoucherRequest.Create(TestData.VoucherId,
                                                                                 TestData.OperatorIdentifier,
                                                                                 TestData.EstateId,
                                                                                 TestData.TransactionId,
                                                                                 TestData.Value,
                                                                                 TestData.RecipientEmail,
                                                                                 TestData.RecipientMobile);

            issueVoucherRequest.ShouldNotBeNull();
            issueVoucherRequest.VoucherId.ShouldBe(TestData.VoucherId);
            issueVoucherRequest.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier);
            issueVoucherRequest.EstateId.ShouldBe(TestData.EstateId);
            issueVoucherRequest.TransactionId.ShouldBe(TestData.TransactionId);
            issueVoucherRequest.Value.ShouldBe(TestData.Value);
            issueVoucherRequest.RecipientEmail.ShouldBe(TestData.RecipientEmail);
            issueVoucherRequest.RecipientMobile.ShouldBe(TestData.RecipientMobile);
        }
    }
}
