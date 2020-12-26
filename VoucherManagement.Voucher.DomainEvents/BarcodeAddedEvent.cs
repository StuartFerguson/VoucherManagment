namespace VoucherManagement.Voucher.DomainEvents
{
    using System;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEvent" />
    [JsonObject]
    public class BarcodeAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodeAddedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="barcode">The barcode.</param>
        public BarcodeAddedEvent(Guid aggregateId,
                                 Guid eventId,
                                 Guid estateId,
                                 String barcode) : base(aggregateId, eventId)
        {
            this.EstateId = estateId;
            this.VoucherId = aggregateId;
            this.Barcode = barcode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the barcode.
        /// </summary>
        /// <value>
        /// The barcode.
        /// </value>
        [JsonProperty]
        public String Barcode { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        [JsonProperty]
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets the voucher identifier.
        /// </summary>
        /// <value>
        /// The voucher identifier.
        /// </value>
        [JsonProperty]
        public Guid VoucherId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="barcode">The barcode.</param>
        /// <returns></returns>
        public static BarcodeAddedEvent Create(Guid aggregateId,
                                               Guid estateId,
                                               String barcode)
        {
            return new BarcodeAddedEvent(aggregateId, Guid.NewGuid(), estateId, barcode);
        }

        #endregion
    }
}