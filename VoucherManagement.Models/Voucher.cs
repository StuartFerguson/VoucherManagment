namespace VoucherManagement.Models
{
    using System;

    public class Voucher
    {
        #region Properties

        /// <summary>
        /// Gets the barcode.
        /// </summary>
        /// <value>
        /// The barcode.
        /// </value>
        public String Barcode { get; set; }

        /// <summary>
        /// Gets the estate identifier.
        /// </summary>
        /// <value>
        /// The estate identifier.
        /// </value>
        public Guid EstateId { get; set; }

        /// <summary>
        /// Gets the expiry date.
        /// </summary>
        /// <value>
        /// The expiry date.
        /// </value>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is generated; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsGenerated { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is issued.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is issued; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsIssued { get; set; }

        /// <summary>
        /// Gets the issued date time.
        /// </summary>
        /// <value>
        /// The issued date time.
        /// </value>
        public DateTime IssuedDateTime { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public String Message { get; set; }

        /// <summary>
        /// The recipient email
        /// </summary>
        public String RecipientEmail { get; set; }

        /// <summary>
        /// The recipient mobile
        /// </summary>
        public String RecipientMobile { get; set; }

        /// <summary>
        /// Gets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Decimal Value { get; set; }

        /// <summary>
        /// Gets the voucher code.
        /// </summary>
        /// <value>
        /// The voucher code.
        /// </value>
        public String VoucherCode { get; set; }

        #endregion
    }
}