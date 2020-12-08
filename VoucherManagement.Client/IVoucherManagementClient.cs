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