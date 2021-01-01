namespace VoucherManagement.BusinessLogic.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateManagement.Client;
    using EstateReporting.Database;
    using EstateReporting.Database.Entities;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Moq;
    using SecurityService.Client;
    using Services;
    using Shared.EntityFramework;
    using Shared.EventStore.EventStore;
    using Shared.Exceptions;
    using Shared.General;
    using Shared.Logger;
    using Shouldly;
    using Testing;
    using VoucherAggregate;
    using Xunit;
    using Voucher = EstateReporting.Database.Entities.Voucher;

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

            EstateReportingContext context = await this.GetContext(Guid.NewGuid().ToString("N"), TestDatabaseType.InMemory);

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object,
                                                                          dbContextFactory.Object);
            

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
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Exception", new KeyNotFoundException("Invalid Estate")));

            EstateReportingContext context = await this.GetContext(Guid.NewGuid().ToString("N"), TestDatabaseType.InMemory);

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object,
                                                                          dbContextFactory.Object);

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

            EstateReportingContext context = await this.GetContext(Guid.NewGuid().ToString("N"), TestDatabaseType.InMemory);

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object,
                                                                          dbContextFactory.Object);

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

            EstateReportingContext context = await this.GetContext(Guid.NewGuid().ToString("N"), TestDatabaseType.InMemory);

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object,
                                                                          dbContextFactory.Object);

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

            EstateReportingContext context = await this.GetContext(Guid.NewGuid().ToString("N"), TestDatabaseType.InMemory);

            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object,
                                                                          dbContextFactory.Object);

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
        public async Task VoucherDomainService_RedeemVoucher_VoucherIssued()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<IAggregateRepository<VoucherAggregate>> voucherAggregateRepository = new Mock<IAggregateRepository<VoucherAggregate>>();
            voucherAggregateRepository.Setup(v => v.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetVoucherAggregateWithRecipientMobile);
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetEstateResponseWithOperator1);

            EstateReportingContext context = await this.GetContext(Guid.NewGuid().ToString("N"), TestDatabaseType.InMemory);
            context.Vouchers.Add(new Voucher
                                 {
                                     VoucherCode = TestData.VoucherCode,
                                     EstateId = TestData.EstateId
                                 });
            await context.SaveChangesAsync();
            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object,
                                                                          dbContextFactory.Object);


            RedeemVoucherResponse redeemVoucherResponse = await domainService.RedeemVoucher(TestData.EstateId, 
                                                                                            TestData.VoucherCode,
                                                                                            TestData.RedeemedDateTime, 
                                                                                            CancellationToken.None);

            redeemVoucherResponse.ShouldNotBeNull();
        }

        [Fact]
        public async Task VoucherDomainService_RedeemVoucher_InvalidEstate_ErrorThrown()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<IAggregateRepository<VoucherAggregate>> voucherAggregateRepository = new Mock<IAggregateRepository<VoucherAggregate>>();
            voucherAggregateRepository.Setup(v => v.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetVoucherAggregateWithRecipientMobile);
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Exception", new KeyNotFoundException("Invalid Estate")));

            EstateReportingContext context = await this.GetContext(Guid.NewGuid().ToString("N"), TestDatabaseType.InMemory);
            context.Vouchers.Add(new Voucher
            {
                VoucherCode = TestData.VoucherCode,
                EstateId = TestData.EstateId
            });
            await context.SaveChangesAsync();
            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object,
                                                                          dbContextFactory.Object);

            Should.Throw<NotFoundException>(async () =>
                                            {
                                                await domainService.RedeemVoucher(TestData.EstateId,
                                                    TestData.VoucherCode,
                                                    TestData.RedeemedDateTime,
                                                    CancellationToken.None);

                                            });
        }

        [Fact]
        public async Task VoucherDomainService_RedeemVoucher_VoucherNotFound_ErrorThrown()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(TestData.DefaultAppSettings).Build();
            ConfigurationReader.Initialise(configurationRoot);

            Logger.Initialise(NullLogger.Instance);

            Mock<IEstateClient> estateClient = new Mock<IEstateClient>();
            Mock<ISecurityServiceClient> securityServiceClient = new Mock<ISecurityServiceClient>();
            Mock<IAggregateRepository<VoucherAggregate>> voucherAggregateRepository = new Mock<IAggregateRepository<VoucherAggregate>>();
            voucherAggregateRepository.Setup(v => v.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetVoucherAggregateWithRecipientMobile);
            securityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.TokenResponse);
            estateClient.Setup(e => e.GetEstate(It.IsAny<String>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetEstateResponseWithOperator1);

            EstateReportingContext context = await this.GetContext(Guid.NewGuid().ToString("N"), TestDatabaseType.InMemory);
            Mock<IDbContextFactory<EstateReportingContext>> dbContextFactory = new Mock<IDbContextFactory<EstateReportingContext>>();
            dbContextFactory.Setup(d => d.GetContext(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(context);

            VoucherDomainService domainService = new VoucherDomainService(voucherAggregateRepository.Object,
                                                                          securityServiceClient.Object,
                                                                          estateClient.Object,
                                                                          dbContextFactory.Object);


            Should.Throw<NotFoundException>(async () =>
                                            {
                                                await domainService.RedeemVoucher(TestData.EstateId,
                                                                                  TestData.VoucherCode,
                                                                                  TestData.RedeemedDateTime,
                                                                                  CancellationToken.None);

                                            });
        }

        private async Task<EstateReportingContext> GetContext(String databaseName, TestDatabaseType databaseType = TestDatabaseType.InMemory)
        {
            EstateReportingContext context = null;
            if (databaseType == TestDatabaseType.InMemory)
            {
                DbContextOptionsBuilder<EstateReportingContext> builder = new DbContextOptionsBuilder<EstateReportingContext>()
                                                                          .UseInMemoryDatabase(databaseName)
                                                                          .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                context = new EstateReportingContext(builder.Options);
            }
            else if (databaseType == TestDatabaseType.SqliteInMemory)
            {
                SqliteConnection inMemorySqlite = new SqliteConnection("Data Source=:memory:");
                inMemorySqlite.Open();

                DbContextOptionsBuilder<EstateReportingContext> builder = new DbContextOptionsBuilder<EstateReportingContext>().UseSqlite(inMemorySqlite);
                context = new EstateReportingContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

            }
            else
            {
                throw new NotSupportedException($"Database type [{databaseType}] not supported");
            }

            return context;
        }

        public enum TestDatabaseType
        {
            InMemory = 0,
            SqliteInMemory = 1
        }
    }
}