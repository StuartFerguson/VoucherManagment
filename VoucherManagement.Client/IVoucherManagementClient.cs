namespace VoucherManagement.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects;

    /// <summary>
    /// 
    /// </summary>
    public interface IVoucherManagementClient
    {
        #region Methods

        /// <summary>
        /// Gets the voucher.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetVoucherResponse> GetVoucher(String accessToken,
                                            Guid estateId,
                                            String voucherCode,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Issues the voucher.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="issueVoucherRequest">The issue voucher request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IssueVoucherResponse> IssueVoucher(String accessToken,
                                                IssueVoucherRequest issueVoucherRequest,
                                                CancellationToken cancellationToken);

        #endregion
    }
}