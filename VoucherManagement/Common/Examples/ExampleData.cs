using System;
using System.Linq;
using System.Threading.Tasks;

namespace VoucherManagement.Common.Examples
{
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// 
    /// </summary>
    internal static class ExampleData
    {
        /// <summary>
        /// The is generated
        /// </summary>
        internal static Boolean IsGenerated = true;
        /// <summary>
        /// The is issued
        /// </summary>
        internal static Boolean IsIssued = true;
        /// <summary>
        /// The is redeemed
        /// </summary>
        internal static Boolean IsRedeemed = true;
        /// <summary>
        /// The estate identifier
        /// </summary>
        internal static Guid EstateId = Guid.Parse("F5C605E3-FFC8-4691-B30C-FDDA235C65A3");

        /// <summary>
        /// The issued date time
        /// </summary>
        internal static DateTime? IssuedDateTime = new DateTime(2021,3,6);

        /// <summary>
        /// The generated date time
        /// </summary>
        internal static DateTime? GeneratedDateTime = new DateTime(2021, 3, 7);
        /// <summary>
        /// The redeemed date time
        /// </summary>
        internal static DateTime? RedeemedDateTime = new DateTime(2021, 3, 7);

        /// <summary>
        /// The operator identifier
        /// </summary>
        internal static String OperatorIdentifier = "Example Voucher Operator";

        /// <summary>
        /// The recipient mobile
        /// </summary>
        internal static String RecipientMobile = "07777777777";
        /// <summary>
        /// The recipient email
        /// </summary>
        internal static String RecipientEmail = "recipient@myvoucheremail.co.uk";

        /// <summary>
        /// The transaction identifier
        /// </summary>
        internal static Guid TransactionId = Guid.Parse("73E4FA61-7955-42A3-99B9-E6AB647C078E");

        /// <summary>
        /// The voucher value
        /// </summary>
        internal static Decimal VoucherValue = 10.00m;
        /// <summary>
        /// The remaining balance
        /// </summary>
        internal static Decimal RemainingBalance = 0;

        /// <summary>
        /// The expiry date
        /// </summary>
        internal static DateTime ExpiryDate = new DateTime(2021, 4, 6);

        /// <summary>
        /// The message
        /// </summary>
        internal static String Message = String.Empty;

        /// <summary>
        /// The voucher code
        /// </summary>
        internal static String VoucherCode = "1234567890";
        /// <summary>
        /// The voucher identifier
        /// </summary>
        internal static Guid VoucherId = Guid.Parse("AD3297AB-5484-4D5E-BBC2-B91815708920");

    }
}
