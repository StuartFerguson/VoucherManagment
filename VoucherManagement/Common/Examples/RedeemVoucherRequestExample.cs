namespace VoucherManagement.Common.Examples
{
    using DataTransferObjects;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.Filters.IExamplesProvider{VoucherManagement.DataTransferObjects.RedeemVoucherRequest}" />
    public class RedeemVoucherRequestExample : IExamplesProvider<RedeemVoucherRequest>
    {
        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <returns></returns>
        public RedeemVoucherRequest GetExamples()
        {
            return new RedeemVoucherRequest
                   {
                       EstateId = ExampleData.EstateId,
                       RedeemedDateTime = ExampleData.RedeemedDateTime,
                       VoucherCode = ExampleData.VoucherCode
                   };
        }
    }
}