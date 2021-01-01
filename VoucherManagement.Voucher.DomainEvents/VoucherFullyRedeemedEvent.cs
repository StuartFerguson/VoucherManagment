using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.Voucher.DomainEvents
{
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    [JsonObject]
    public class VoucherFullyRedeemedEvent : DomainEvent
    {
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

        /// <summary>
        /// Gets the redeemed date time.
        /// </summary>
        /// <value>
        /// The redeemed date time.
        /// </value>
        [JsonProperty]
        public DateTime RedeemedDateTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherIssuedEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public VoucherFullyRedeemedEvent()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherFullyRedeemedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="redeemedDateTime">The redeemed date time.</param>
        private VoucherFullyRedeemedEvent(Guid aggregateId,
                                          Guid eventId,
                                          Guid estateId,
                                          DateTime redeemedDateTime) : base(aggregateId, eventId)
        {
            this.EstateId = estateId;
            this.RedeemedDateTime = redeemedDateTime;
            this.VoucherId = aggregateId;
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="redeemedDateTime">The redeemed date time.</param>
        /// <returns></returns>
        public static VoucherFullyRedeemedEvent Create(Guid aggregateId,
                                                       Guid estateId,
                                                       DateTime redeemedDateTime)
        {
            return new VoucherFullyRedeemedEvent(aggregateId,Guid.NewGuid(), estateId,redeemedDateTime);
        }
    }
}
