namespace VoucherManagement.Common.Examples
{
    using DataTransferObjects;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.Filters.IExamplesProvider{VoucherManagement.DataTransferObjects.RedeemVoucherResponse}" />
    public class RedeemVoucherResponseExample : IExamplesProvider<RedeemVoucherResponse>
    {
        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <returns></returns>
        public RedeemVoucherResponse GetExamples()
        {
            return new RedeemVoucherResponse
                   {
                       VoucherCode = ExampleData.VoucherCode,
                       ExpiryDate = ExampleData.ExpiryDate,
                       RemainingBalance = ExampleData.RemainingBalance
                   };
        }
    }
}