using System;

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
    public class VoucherGeneratedEvent : DomainEvent
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
        /// Gets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        [JsonProperty]
        public String OperatorIdentifier { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty]
        public Decimal Value { get; private set; }

        /// <summary>
        /// Gets the voucher code.
        /// </summary>
        /// <value>
        /// The voucher code.
        /// </value>
        [JsonProperty]
        public String VoucherCode { get; private set; }

        /// <summary>
        /// Gets the expiry date time.
        /// </summary>
        /// <value>
        /// The expiry date time.
        /// </value>
        [JsonProperty]
        public DateTime ExpiryDateTime { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty]
        public String Message { get; private set; }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="expiryDateTime">The expiry date time.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static VoucherGeneratedEvent Create(Guid aggregateId,
                                                   Guid estateId,
                                                   String operatorIdentifier,
                                                   Decimal value,
                                                   String voucherCode,
                                                   DateTime expiryDateTime,
                                                   String message)
        {
            return new VoucherGeneratedEvent(aggregateId,Guid.NewGuid(), estateId,operatorIdentifier,value,voucherCode, expiryDateTime, message);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherGeneratedEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public VoucherGeneratedEvent()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherGeneratedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="value">The value.</param>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="expiryDateTime">The expiry date time.</param>
        /// <param name="message">The message.</param>
        private VoucherGeneratedEvent(Guid aggregateId,
                                      Guid eventId,
                                      Guid estateId,
                                      String operatorIdentifier,
                                      Decimal value,
                                      String voucherCode,
                                      DateTime expiryDateTime,
            String message) :base(aggregateId, eventId)
        {
            this.EstateId = estateId;
            this.OperatorIdentifier = operatorIdentifier;
            this.Value = value;
            this.VoucherCode = voucherCode;
            this.ExpiryDateTime = expiryDateTime;
            this.Message = message;
            this.VoucherId = aggregateId;
        }
    }
}
