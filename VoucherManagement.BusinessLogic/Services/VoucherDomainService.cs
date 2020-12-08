namespace VoucherManagement.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateManagement.Client;
    using EstateManagement.DataTransferObjects.Responses;
    using Models;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects.Responses;
    using Shared.EventStore.EventStore;
    using Shared.Exceptions;
    using Shared.General;
    using Shared.Logger;
    using VoucherAggregate;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="VoucherManagement.BusinessLogic.Services.IVoucherDomainService" />
    public class VoucherDomainService : IVoucherDomainService
    {
        private readonly IAggregateRepository<VoucherAggregate> VoucherAggregateRepository;

        /// <summary>
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

        /// <summary>
        /// The estate client
        /// </summary>
        private readonly IEstateClient EstateClient;

        #region Constructors

        public VoucherDomainService(IAggregateRepository<VoucherAggregate> voucherAggregateRepository,
                                    ISecurityServiceClient securityServiceClient,
                                    IEstateClient estateClient)
        {
            this.VoucherAggregateRepository = voucherAggregateRepository;
            this.SecurityServiceClient = securityServiceClient;
            this.EstateClient = estateClient;
        }
        #endregion

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
        public async Task<IssueVoucherResponse> IssueVoucher(Guid voucherId, String operatorId, Guid estateId, Decimal value, 
                                                             String recipientEmail, String recipientMobile,CancellationToken cancellationToken)
        {
            await this.ValidateVoucherIssue(estateId, operatorId, cancellationToken);

            VoucherAggregate voucher = await this.VoucherAggregateRepository.GetLatestVersion(voucherId, cancellationToken);

            voucher.Generate(operatorId,estateId,value);
            voucher.Issue(recipientEmail,recipientMobile);

            await this.VoucherAggregateRepository.SaveChanges(voucher, cancellationToken);

            return new IssueVoucherResponse
                   {
                       ExpiryDate = voucher.ExpiryDate,
                       Message = voucher.Message,
                       VoucherCode = voucher.VoucherCode,
                       VoucherId = voucherId
                   };
        }

        /// <summary>
        /// Validates the voucher issue.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Estate Id [{estateId}] is not a valid estate
        /// or
        /// Operator Identifier [{operatorIdentifier}] is not a valid for estate [{estate.EstateName}]
        /// </exception>
        private async Task<EstateResponse> ValidateVoucherIssue(Guid estateId, String operatorIdentifier, CancellationToken cancellationToken)
        {
            EstateResponse estate = null;

            // Validate the Estate Record is a valid estate
            try
            {
                estate = await this.GetEstate(estateId, cancellationToken);
            }
            catch (Exception ex) when (ex.InnerException != null && ex.InnerException.GetType() == typeof(KeyNotFoundException))
            {
                throw new NotFoundException($"Estate Id [{estateId}] is not a valid estate");
            }

            if (estate.Operators == null || estate.Operators.Any() == false)
            {
                throw new NotFoundException($"Estate {estate.EstateName} has no operators defined");
            }

            EstateOperatorResponse estateOperator = estate.Operators.SingleOrDefault(o => o.Name == operatorIdentifier);
            if (estateOperator == null)
            {
                throw new NotFoundException($"Operator Identifier [{operatorIdentifier}] is not a valid for estate [{estate.EstateName}]");
            }

            return estate;
        }

        /// <summary>
        /// The token response
        /// </summary>
        private TokenResponse TokenResponse;

        private async Task<EstateResponse> GetEstate(Guid estateId,
                                                     CancellationToken cancellationToken)
        {
            this.TokenResponse = await this.GetToken(cancellationToken);

            EstateResponse estate = await this.EstateClient.GetEstate(this.TokenResponse.AccessToken, estateId, cancellationToken);

            return estate;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task<TokenResponse> GetToken(CancellationToken cancellationToken)
        {
            // Get a token to talk to the estate service
            String clientId = ConfigurationReader.GetValue("AppSettings", "ClientId");
            String clientSecret = ConfigurationReader.GetValue("AppSettings", "ClientSecret");
            Logger.LogInformation($"Client Id is {clientId}");
            Logger.LogInformation($"Client Secret is {clientSecret}");

            if (this.TokenResponse == null)
            {
                TokenResponse token = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
                Logger.LogInformation($"Token is {token.AccessToken}");
                return token;
            }

            if (this.TokenResponse.Expires.UtcDateTime.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(2))
            {
                Logger.LogInformation($"Token is about to expire at {this.TokenResponse.Expires.DateTime:O}");
                TokenResponse token = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
                Logger.LogInformation($"Token is {token.AccessToken}");
                return token;
            }

            return this.TokenResponse;
        }

        #endregion
    }
}