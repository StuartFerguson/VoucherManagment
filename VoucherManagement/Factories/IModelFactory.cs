namespace VoucherManagement.Factories
{
    using DataTransferObjects;

    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="issueVoucherResponse">The issue voucher response.</param>
        /// <returns></returns>
        IssueVoucherResponse ConvertFrom(Models.IssueVoucherResponse issueVoucherResponse);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="voucherModel">The voucher model.</param>
        /// <returns></returns>
        GetVoucherResponse ConvertFrom(Models.Voucher voucherModel);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="redeemVoucherResponse">The redeem voucher response.</param>
        /// <returns></returns>
        RedeemVoucherResponse ConvertFrom(Models.RedeemVoucherResponse redeemVoucherResponse);

        #endregion
    }
}