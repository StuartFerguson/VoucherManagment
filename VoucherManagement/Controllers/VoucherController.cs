using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace VoucherManagement.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Common;
    using DataTransferObjects;
    using Factories;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;

    [ExcludeFromCodeCoverage]
    [Route(VoucherController.ControllerRoute)]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class VoucherController : ControllerBase
    {
        private readonly IMediator Mediator;

        private readonly IModelFactory ModelFactory;

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "vouchers";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + VoucherController.ControllerName;

        #endregion

        [HttpPost]
        public async Task<IActionResult> IssueVoucher(IssueVoucherRequest issueVoucherRequest,  CancellationToken cancellationToken)
        {
            // Reject password tokens
            if (ClaimsHelper.IsPasswordToken(this.User))
            {
                return this.Forbid();
            }

            Guid voucherId = Guid.NewGuid();
            DateTime issuedDateTime = issueVoucherRequest.IssuedDateTime.HasValue ? issueVoucherRequest.IssuedDateTime.Value : DateTime.Now;

            BusinessLogic.Requests.IssueVoucherRequest request = BusinessLogic.Requests.IssueVoucherRequest.Create(voucherId,
                issueVoucherRequest.OperatorIdentifier,
                issueVoucherRequest.EstateId,
                issueVoucherRequest.TransactionId,
                issuedDateTime,
                issueVoucherRequest.Value,
                issueVoucherRequest.RecipientEmail,
                issueVoucherRequest.RecipientMobile);

            Models.IssueVoucherResponse response = await this.Mediator.Send(request, cancellationToken);
            
            // TODO: Populate the GET route
            return this.Created("", this.ModelFactory.ConvertFrom(response));
        }

        public VoucherController(IMediator mediator,
                                 IModelFactory modelFactory)
        {
            this.Mediator = mediator;
            this.ModelFactory = modelFactory;
        }
    }
}
