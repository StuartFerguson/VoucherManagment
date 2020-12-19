namespace VoucherManagement.BusinessLogic.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateManagement.Client;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Moq;
    using SecurityService.Client;
    using Services;
    using Shared.EventStore.EventStore;
    using Shared.Exceptions;
    using Shared.General;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using VoucherAggregate;
    using Xunit;

    public class VoucherDomainServiceTests
    {
        [Fact]
        public async Task VoucherDomainService_IssueVoucher_VoucherIssued()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);
            
            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<IAggregateRepository<VoucherAggregate>> voucherAggregateRepository = new Mock<IAggregateRepository<VoucherAggregate>>();
            voucherAggregateRepository.Setup(v => v.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new VoucherAggregate());
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetEstateResponseWithOperator1);
            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object);
            

            IssueVoucherResponse issueVoucherResponse = await domainService.IssueVoucher(TestData.VoucherId, TestData.OperatorIdentifier,
                                                                                         TestData.EstateId, TestData.TransactionId,
                                                                                         TestData.IssuedDateTime, TestData.Value,
                                                                                         TestData.RecipientEmail,TestData.RecipientMobile,
                                                                                         CancellationToken.None);

            issueVoucherResponse.ShouldNotBeNull();
        }

        [Fact]
        public async Task VoucherDomainService_IssueVoucher_InvalidEstate_ErrorThrown()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<IAggregateRepository<VoucherAggregate>> voucherAggregateRepository = new Mock<IAggregateRepository<VoucherAggregate>>();
            voucherAggregateRepository.Setup(v => v.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new VoucherAggregate());
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);
            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object);
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Exception", new KeyNotFoundException("Invalid Estate")));

            Should.Throw<NotFoundException>(async () =>
                                            {
                                                await domainService.IssueVoucher(TestData.VoucherId,
                                                                                 TestData.OperatorIdentifier,
                                                                                 TestData.EstateId, TestData.TransactionId,
                                                                                 TestData.IssuedDateTime, TestData.Value,
                                                                                 TestData.RecipientEmail,
                                                                                 TestData.RecipientMobile,
                                                                                 CancellationToken.None);
                                            });

        }

        [Fact]
        public async Task VoucherDomainService_IssueVoucher_EstateWithEmptyOperators_ErrorThrown()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<IAggregateRepository<VoucherAggregate>> voucherAggregateRepository = new Mock<IAggregateRepository<VoucherAggregate>>();
            voucherAggregateRepository.Setup(v => v.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new VoucherAggregate());
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetEstateResponseWithEmptyOperators);
            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object);

            Should.Throw<NotFoundException>(async () =>
            {
                await domainService.IssueVoucher(TestData.VoucherId,
                                                 TestData.OperatorIdentifier,
                                                 TestData.EstateId, TestData.TransactionId,
                                                 TestData.IssuedDateTime, TestData.Value,
                                                 TestData.RecipientEmail,
                                                 TestData.RecipientMobile,
                                                 CancellationToken.None);
            });

        }

        [Fact]
        public async Task VoucherDomainService_IssueVoucher_EstateWithNullOperators_ErrorThrown()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<IAggregateRepository<VoucherAggregate>> voucherAggregateRepository = new Mock<IAggregateRepository<VoucherAggregate>>();
            voucherAggregateRepository.Setup(v => v.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new VoucherAggregate());
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetEstateResponseWithNullOperators);
            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object);
            
            Should.Throw<NotFoundException>(async () =>
            {
                await domainService.IssueVoucher(TestData.VoucherId,
                                                 TestData.OperatorIdentifier,
                                                 TestData.EstateId, TestData.TransactionId,
                                                 TestData.IssuedDateTime, TestData.Value,
                                                 TestData.RecipientEmail,
                                                 TestData.RecipientMobile,
                                                 CancellationToken.None);
            });

        }

        [Fact]
        public async Task VoucherDomainService_IssueVoucher_OperatorNotSupportedByEstate_ErrorThrown()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<IAggregateRepository<VoucherAggregate>> voucherAggregateRepository = new Mock<IAggregateRepository<VoucherAggregate>>();
            voucherAggregateRepository.Setup(v => v.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new VoucherAggregate());
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetEstateResponseWithOperator2);
            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object);

            Should.Throw<NotFoundException>(async () =>
            {
                await domainService.IssueVoucher(TestData.VoucherId,
                                                 TestData.OperatorIdentifier,
                                                 TestData.EstateId, TestData.TransactionId,
                                                 TestData.IssuedDateTime, TestData.Value,
                                                 TestData.RecipientEmail,
                                                 TestData.RecipientMobile,
                                                 CancellationToken.None);
            });

        }
    }
}