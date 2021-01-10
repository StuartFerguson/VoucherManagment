namespace VoucherManagement.BusinessLogic.Manager
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    public interface IVoucherManagementManager
    {
        #region Methods

        /// <summary>
        /// Gets the voucher by code.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Voucher> GetVoucherByCode(Guid estateId,
                                       String voucherCode,
                                       CancellationToken cancellationToken);

        #endregion
    }
}