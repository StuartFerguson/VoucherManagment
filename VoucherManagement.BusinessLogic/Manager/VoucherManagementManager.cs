namespace VoucherManagement.BusinessLogic.Manager
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using EstateReporting.Database;
    using Models;
    using Shared.EntityFramework;
    using Shared.EventStore.EventStore;
    using Shared.Exceptions;
    using VoucherAggregate;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="VoucherManagement.BusinessLogic.Manager.IVoucherManagementManager" />
    public class VoucherManagementManager : IVoucherManagementManager
    {
        #region Fields

        /// <summary>
        /// The database context factory
        /// </summary>
        private readonly IDbContextFactory<EstateReportingContext> DbContextFactory;

        /// <summary>
        /// The voucher aggregate repository
        /// </summary>
        private readonly IAggregateRepository<VoucherAggregate> VoucherAggregateRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherManagementManager"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        /// <param name="voucherAggregateRepository">The voucher aggregate repository.</param>
        public VoucherManagementManager(IDbContextFactory<EstateReportingContext> dbContextFactory,
                                        IAggregateRepository<VoucherAggregate> voucherAggregateRepository)
        {
            this.DbContextFactory = dbContextFactory;
            this.VoucherAggregateRepository = voucherAggregateRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the voucher by code.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Voucher not found with Voucher Code [{voucherCode}]</exception>
        public async Task<Voucher> GetVoucherByCode(Guid estateId,
                                                    String voucherCode,
                                                    CancellationToken cancellationToken)
        {
            EstateReportingContext context = await this.DbContextFactory.GetContext(estateId, cancellationToken);

            EstateReporting.Database.Entities.Voucher voucher = await context.Vouchers.SingleOrDefaultAsync(v => v.VoucherCode == voucherCode, cancellationToken);

            if (voucher == null)
            {
                throw new NotFoundException($"Voucher not found with Voucher Code [{voucherCode}]");
            }

            // Get the aggregate
            VoucherAggregate voucherAggregate = await this.VoucherAggregateRepository.GetLatestVersion(voucher.VoucherId, cancellationToken);

            return voucherAggregate.GetVoucher();
        }

        #endregion
    }
}