namespace VoucherManagement.Common.Examples
{
    using DataTransferObjects;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.Filters.IExamplesProvider{VoucherManagement.DataTransferObjects.IssueVoucherResponse}" />
    public class IssueVoucherResponseExample : IExamplesProvider<IssueVoucherResponse>
    {
        #region Methods

        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <returns></returns>
        public IssueVoucherResponse GetExamples()
        {
            return new IssueVoucherResponse
                   {
                       ExpiryDate = ExampleData.ExpiryDate,
                       Message = ExampleData.Message,
                       VoucherCode = ExampleData.VoucherCode,
                       VoucherId = ExampleData.VoucherId
                   };
        }

        #endregion
    }
}