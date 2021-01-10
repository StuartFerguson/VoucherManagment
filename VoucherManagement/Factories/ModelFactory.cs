namespace VoucherManagement.Factories
{
    using DataTransferObjects;
    using Microsoft.Rest;
    using Models;
    using IssueVoucherResponse = DataTransferObjects.IssueVoucherResponse;

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

        public GetVoucherResponse ConvertFrom(Voucher voucherModel)
        {
            if (voucherModel == null)
            {
                return null;
            }

            GetVoucherResponse response = new GetVoucherResponse
                                          {
                                              Value = voucherModel.Value,
                                              Balance = voucherModel.Balance,
                                              ExpiryDate = voucherModel.ExpiryDate,
                                              VoucherCode = voucherModel.VoucherCode,
                                              TransactionId = voucherModel.TransactionId,
                                              IssuedDateTime = voucherModel.IssuedDateTime,
                                              IsIssued = voucherModel.IsIssued,
                                              GeneratedDateTime = voucherModel.GeneratedDateTime,
                                              IsGenerated = voucherModel.IsGenerated,
                                              IsRedeemed = voucherModel.IsRedeemed,
                                              RedeemedDateTime = voucherModel.RedeemedDateTime
                                          };

            return response;
        }

        #endregion
    }
}