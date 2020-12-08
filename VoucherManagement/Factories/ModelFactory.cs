namespace VoucherManagement.Factories
{
    using DataTransferObjects;
    using Microsoft.Rest;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="VoucherManagement.Factories.IModelFactory" />
    public class ModelFactory : IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="voucherResponse">The voucher response.</param>
        /// <returns></returns>
        public IssueVoucherResponse ConvertFrom(Models.IssueVoucherResponse voucherResponse)
        {
            if (voucherResponse == null)
            {
                return null;
            }

            IssueVoucherResponse response = new IssueVoucherResponse
                                            {
                                                Message = voucherResponse.Message,
                                                ExpiryDate = voucherResponse.ExpiryDate,
                                                VoucherCode = voucherResponse.VoucherCode,
                                                VoucherId = voucherResponse.VoucherId
                                            };

            return response;


        }

        #endregion
    }
}