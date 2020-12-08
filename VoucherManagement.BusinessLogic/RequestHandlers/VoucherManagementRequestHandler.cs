﻿namespace VoucherManagement.BusinessLogic.RequestHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Requests;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler{VoucherManagement.BusinessLogic.Requests.IssueVoucherRequest, VoucherManagement.Models.IssueVoucherResponse}" />
    public class VoucherManagementRequestHandler : IRequestHandler<IssueVoucherRequest, IssueVoucherResponse>
    {
        #region Fields

        /// <summary>
        /// The voucher domain service
        /// </summary>
        private readonly IVoucherDomainService VoucherDomainService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherManagementRequestHandler"/> class.
        /// </summary>
        /// <param name="voucherDomainService">The voucher domain service.</param>
        public VoucherManagementRequestHandler(IVoucherDomainService voucherDomainService)
        {
            this.VoucherDomainService = voucherDomainService;
        }

        #endregion

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        public async Task<IssueVoucherResponse> Handle(IssueVoucherRequest request,
                                                       CancellationToken cancellationToken)
        {
            return await this.VoucherDomainService.IssueVoucher(request.VoucherId, request.OperatorIdentifier, request.EstateId, request.Value,
                                                                request.RecipientEmail, request.RecipientMobile, cancellationToken);
        }
    }
}