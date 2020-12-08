using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.Voucher.DomainEvents
{
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.DomainDrivenDesign.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.DomainDrivenDesign.EventSourcing.DomainEvent" />
    [JsonObject]
    public class VoucherIssuedEvent : DomainEvent
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
        /// Gets the recipient email.
        /// </summary>
        /// <value>
        /// The recipient email.
        /// </value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String RecipientEmail { get; private set; }

        /// <summary>
        /// Gets the recipient mobile.
        /// </summary>
        /// <value>
        /// The recipient mobile.
        /// </value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String RecipientMobile { get; private set; }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="recipientEmail">The recipient email.</param>
        /// <param name="recipientMobile">The recipient mobile.</param>
        /// <returns></returns>
        public static VoucherIssuedEvent Create(Guid aggregateId,
                                                Guid estateId,
                                                String recipientEmail,
                                                String recipientMobile)
        {
            return new VoucherIssuedEvent(aggregateId, Guid.NewGuid(), estateId, recipientEmail, recipientMobile);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherIssuedEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public VoucherIssuedEvent()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherIssuedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="recipientEmail">The recipient email.</param>
        /// <param name="recipientMobile">The recipient mobile.</param>
        private VoucherIssuedEvent(Guid aggregateId,
                                      Guid eventId,
                                      Guid estateId,
                                      String recipientEmail,
                                      String recipientMobile) : base(aggregateId, eventId)
        {
            this.EstateId = estateId;
            this.VoucherId = aggregateId;
            this.RecipientEmail = recipientEmail;
            this.RecipientMobile = recipientMobile;
        }
    }
}
