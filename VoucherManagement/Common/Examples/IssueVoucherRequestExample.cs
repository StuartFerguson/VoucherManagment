namespace VoucherManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.Filters.IMultipleExamplesProvider{VoucherManagement.DataTransferObjects.IssueVoucherRequest}" />
    public class IssueVoucherRequestExample : IMultipleExamplesProvider<IssueVoucherRequest>
    {
        #region Methods

        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SwaggerExample<IssueVoucherRequest>> GetExamples()
        {
            SwaggerExample<IssueVoucherRequest> voucherIssueToEmail = new SwaggerExample<IssueVoucherRequest>
                                                                      {
                                                                          Name = "Issue Voucher to Email Address",
                                                                          Value = new IssueVoucherRequest
                                                                                  {
                                                                                      EstateId = ExampleData.EstateId,
                                                                                      IssuedDateTime = ExampleData.IssuedDateTime,
                                                                                      OperatorIdentifier = ExampleData.OperatorIdentifier,
                                                                                      RecipientEmail = ExampleData.RecipientEmail,
                                                                                      TransactionId = ExampleData.TransactionId,
                                                                                      Value = ExampleData.VoucherValue
                                                                                  }
                                                                      };

            SwaggerExample<IssueVoucherRequest> voucherIssueToMobile = new SwaggerExample<IssueVoucherRequest>
                                                                       {
                                                                           Name = "Issue Voucher to Mobile Number",
                                                                           Value = new IssueVoucherRequest
                                                                                   {
                                                                                       EstateId = ExampleData.EstateId,
                                                                                       IssuedDateTime = ExampleData.IssuedDateTime,
                                                                                       OperatorIdentifier = ExampleData.OperatorIdentifier,
                                                                                       RecipientMobile = ExampleData.RecipientMobile,
                                                                                       TransactionId = ExampleData.TransactionId,
                                                                                       Value = ExampleData.VoucherValue
                                                                                   }
                                                                       };

            return new List<SwaggerExample<IssueVoucherRequest>>
                   {
                       voucherIssueToEmail,
                       voucherIssueToMobile
                   };
        }

        #endregion
    }
}