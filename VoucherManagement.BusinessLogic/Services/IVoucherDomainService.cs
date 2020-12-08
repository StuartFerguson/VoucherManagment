namespace VoucherManagement.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    public interface IVoucherDomainService
    {
        #region Methods

        /// <summary>
        /// Issues the voucher.
        /// </summary>
        /// <param name="voucherId">The voucher identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="recipientEmail">The recipient email.</param>
        /// <param name="recipientMobile">The recipient mobile.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IssueVoucherResponse> IssueVoucher(Guid voucherId,
                                                String operatorId,
                                                Guid estateId,
                                                Decimal value,
                                                String recipientEmail,
                                                String recipientMobile,
                                                CancellationToken cancellationToken);

        #endregion
    }
}