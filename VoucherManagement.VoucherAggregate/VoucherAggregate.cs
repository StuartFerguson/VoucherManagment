namespace VoucherManagement.VoucherAggregate
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Shared.DomainDrivenDesign.EventSourcing;
    using Shared.EventStore.EventStore;
    using Shared.General;
    using Voucher.DomainEvents;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventStore.EventStore.Aggregate" />
    public class VoucherAggregate : Aggregate
    {
        #region Fields

        /// <summary>
        /// The random
        /// </summary>
        private static readonly Random _random = new Random();

        private static readonly Random rdm = new Random();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public VoucherAggregate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private VoucherAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the barcode.
        /// </summary>
        /// <value>
        /// The barcode.
        /// </value>
        public String Barcode { get; private set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; private set; }

        /// <summary>
        /// Gets the expiry date.
        /// </summary>
        /// <value>
        /// The expiry date.
        /// </value>
        public DateTime ExpiryDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is generated; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsGenerated { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is issued.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is issued; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsIssued { get; private set; }

        /// <summary>
        /// Gets the issued date time.
        /// </summary>
        /// <value>
        /// The issued date time.
        /// </value>
        public DateTime IssuedDateTime { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public String Message { get; private set; }

        /// <summary>
        /// Gets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; private set; }

        /// <summary>
        /// Gets the voucher code.
        /// </summary>
        /// <value>
        /// The voucher code.
        /// </value>
        public String VoucherCode { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the barcode.
        /// </summary>
        /// <param name="barcodeAsBase64">The barcode as base64.</param>
        public void AddBarcode(String barcodeAsBase64)
        {
            Guard.ThrowIfNullOrEmpty(barcodeAsBase64, nameof(barcodeAsBase64));

            this.CheckIfVoucherHasBeenGenerated();
            this.CheckIfVoucherHasBeenIssued();

            BarcodeAddedEvent barcodeAddedEvent = BarcodeAddedEvent.Create(this.AggregateId, this.EstateId, barcodeAsBase64);

            this.ApplyAndPend(barcodeAddedEvent);
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static VoucherAggregate Create(Guid aggregateId)
        {
            return new VoucherAggregate(aggregateId);
        }

        /// <summary>
        /// Generates the specified operator identifier.
        /// </summary>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="issuedDateTime">The issued date time.</param>
        /// <param name="value">The value.</param>
        public void Generate(String operatorIdentifier,
                             Guid estateId,
                             Guid transactionId,
                             DateTime issuedDateTime,
                             Decimal value)
        {
            Guard.ThrowIfInvalidDate(issuedDateTime, nameof(issuedDateTime));
            Guard.ThrowIfNullOrEmpty(operatorIdentifier, nameof(operatorIdentifier));
            Guard.ThrowIfInvalidGuid(transactionId, nameof(transactionId));
            Guard.ThrowIfInvalidGuid(estateId, nameof(estateId));
            Guard.ThrowIfNegative(value, nameof(value));
            Guard.ThrowIfZero(value, nameof(value));
            this.CheckIfVoucherAlreadyGenerated();

            // Do the generate process here...
            String voucherCode = this.GenerateVoucherCode();
            DateTime expiryDateTime = issuedDateTime.AddDays(30); // Default to a 30 day expiry for now...
            String message = string.Empty;

            VoucherGeneratedEvent voucherGeneratedEvent =
                VoucherGeneratedEvent.Create(this.AggregateId, estateId, transactionId, issuedDateTime, operatorIdentifier, value, voucherCode, expiryDateTime, message);

            this.ApplyAndPend(voucherGeneratedEvent);
        }

        /// <summary>
        /// Issues the specified recipient email.
        /// </summary>
        /// <param name="recipientEmail">The recipient email.</param>
        /// <param name="recipientMobile">The recipient mobile.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Issue(String recipientEmail,
                          String recipientMobile)
        {
            this.CheckIfVoucherHasBeenGenerated();

            if (string.IsNullOrEmpty(recipientEmail) && string.IsNullOrEmpty(recipientMobile))
            {
                throw new ArgumentNullException(message:"Either Recipient Email Address or Recipient Mobile number must be set to issue a voucher", innerException:null);
            }

            this.CheckIfVoucherAlreadyIssued();

            VoucherIssuedEvent voucherIssuedEvent = VoucherIssuedEvent.Create(this.AggregateId, this.EstateId, this.IssuedDateTime, recipientEmail, recipientMobile);

            this.ApplyAndPend(voucherIssuedEvent);
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        protected override Object GetMetadata()
        {
            return new
                   {
                       this.EstateId
                   };
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        /// <summary>
        /// Checks if voucher already generated.
        /// </summary>
        /// <exception cref="InvalidOperationException">Voucher Id [{this.AggregateId}] has already been generated</exception>
        private void CheckIfVoucherAlreadyGenerated()
        {
            if (this.IsGenerated)
            {
                throw new InvalidOperationException($"Voucher Id [{this.AggregateId}] has already been generated");
            }
        }

        /// <summary>
        /// Checks if voucher already issued.
        /// </summary>
        /// <exception cref="InvalidOperationException">Voucher Id [{this.AggregateId}] has already been issued</exception>
        private void CheckIfVoucherAlreadyIssued()
        {
            if (this.IsIssued)
            {
                throw new InvalidOperationException($"Voucher Id [{this.AggregateId}] has already been issued");
            }
        }

        /// <summary>
        /// Checks if voucher has been issued.
        /// </summary>
        /// <exception cref="InvalidOperationException">Voucher Id [{this.AggregateId}] has not been issued</exception>
        private void CheckIfVoucherHasBeenIssued()
        {
            if (this.IsIssued == false)
            {
                throw new InvalidOperationException($"Voucher Id [{this.AggregateId}] has not been issued");
            }
        }

        /// <summary>
        /// Checks if voucher has been generated.
        /// </summary>
        /// <exception cref="InvalidOperationException">Voucher Id [{this.AggregateId}] has not been generated</exception>
        private void CheckIfVoucherHasBeenGenerated()
        {
            if (this.IsGenerated == false)
            {
                throw new InvalidOperationException($"Voucher Id [{this.AggregateId}] has not been generated");
            }
        }

        /// <summary>
        /// Generates the voucher code.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        private String GenerateVoucherCode(Int32 length = 10)
        {
            // validate length to be greater than 0
            if (length <= 1) length = 10;

            Int32 min = (Int32)Math.Pow(10, length - 1);
            Int32 max = (Int32)Math.Pow(10, length) - 1;

            return VoucherAggregate.rdm.Next(min, max).ToString();
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(VoucherGeneratedEvent domainEvent)
        {
            this.IsGenerated = true;
            this.EstateId = domainEvent.EstateId;
            this.IssuedDateTime = domainEvent.GeneratedDateTime;
            this.ExpiryDate = domainEvent.ExpiryDateTime;
            this.VoucherCode = domainEvent.VoucherCode;
            this.Message = domainEvent.Message;
            this.TransactionId = domainEvent.TransactionId;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(VoucherIssuedEvent domainEvent)
        {
            this.IsIssued = true;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(BarcodeAddedEvent domainEvent)
        {
            this.Barcode = domainEvent.Barcode;
        }

        #endregion
    }
}