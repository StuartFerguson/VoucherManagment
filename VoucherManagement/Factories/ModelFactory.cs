namespace VoucherManagement.Factories
{
    using DataTransferObjects;
    using Models;
    using IssueVoucherResponse = DataTransferObjects.IssueVoucherResponse;
    using RedeemVoucherResponse = DataTransferObjects.RedeemVoucherResponse;

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
        /// <param name="issueVoucherResponse">The issue voucher response.</param>
        /// <returns></returns>
        public IssueVoucherResponse ConvertFrom(Models.IssueVoucherResponse issueVoucherResponse)
        {
            if (issueVoucherResponse == null)
            {
                return null;
            }

            IssueVoucherResponse response = new IssueVoucherResponse
                                            {
                                                Message = issueVoucherResponse.Message,
                                                ExpiryDate = issueVoucherResponse.ExpiryDate,
                                                VoucherCode = issueVoucherResponse.VoucherCode,
                                                VoucherId = issueVoucherResponse.VoucherId
                                            };

            return response;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="voucherModel">The voucher model.</param>
        /// <returns></returns>
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
                                              RedeemedDateTime = voucherModel.RedeemedDateTime,
                                              VoucherId = voucherModel.VoucherId
                                          };

            return response;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="redeemVoucherResponse">The redeem voucher response.</param>
        /// <returns></returns>
        public RedeemVoucherResponse ConvertFrom(Models.RedeemVoucherResponse redeemVoucherResponse)
        {
            if (redeemVoucherResponse == null)
            {
                return null;
            }

            RedeemVoucherResponse response = new RedeemVoucherResponse
                                             {
                                                 ExpiryDate = redeemVoucherResponse.ExpiryDate,
                                                 VoucherCode = redeemVoucherResponse.VoucherCode,
                                                 RemainingBalance = redeemVoucherResponse.RemainingBalance
                                             };

            return response;
        }

        #endregion
    }
}