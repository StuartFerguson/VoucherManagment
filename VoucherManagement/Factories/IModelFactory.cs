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
        /// <param name="voucherResponse">The voucher response.</param>
        /// <returns></returns>
        IssueVoucherResponse ConvertFrom(Models.IssueVoucherResponse voucherResponse);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="voucherModel">The voucher model.</param>
        /// <returns></returns>
        GetVoucherResponse ConvertFrom(Models.Voucher voucherModel);

        #endregion
    }
}