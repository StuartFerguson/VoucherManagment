using System;
using Xunit;

namespace VoucherManagement.Testing
{
    using System.Collections.Generic;
    using BusinessLogic.Requests;
    using EstateManagement.DataTransferObjects.Responses;
    using Models;
    using SecurityService.DataTransferObjects.Responses;
    using Voucher.DomainEvents;
    using VoucherAggregate;

    public static class TestData
    {
        public static IReadOnlyDictionary<String, String> DefaultAppSettings =>
            new Dictionary<String, String>
            {
                ["AppSettings:ClientId"] = "clientId",
                ["AppSettings:ClientSecret"] = "clientSecret"
            };

        public static TokenResponse TokenResponse()
        {
            return SecurityService.DataTransferObjects.Responses.TokenResponse.Create("AccessToken", string.Empty, 100);
        }

        public static String EstateName = "Test Estate 1";
        public static Boolean RequireCustomMerchantNumber = true;

        public static Boolean RequireCustomTerminalNumber = true;

        public static Guid OperatorId = Guid.Parse("804E9D8D-C6FE-4A46-9E55-6A04EA3E1AE5");

        public static EstateResponse GetEstateResponseWithNullOperators =>
            new EstateResponse
            {
                EstateName = TestData.EstateName,
                EstateId = TestData.EstateId,
                Operators = null
            };

        public static EstateResponse GetEstateResponseWithEmptyOperators =>
            new EstateResponse
            {
                EstateName = TestData.EstateName,
                EstateId = TestData.EstateId,
                Operators = new List<EstateOperatorResponse>()
            };

        public static EstateResponse GetEstateResponseWithOperator1 =>
            new EstateResponse
            {
                EstateName = TestData.EstateName,
                EstateId = TestData.EstateId,
                Operators = new List<EstateOperatorResponse>
                            {
                                new EstateOperatorResponse
                                {
                                    Name = TestData.OperatorIdentifier,
                                    OperatorId = TestData.OperatorId,
                                    RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                                    RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber
                                }
                            }
            };

        public static String OperatorIdentifier2 = "NotSupported";

        public static EstateResponse GetEstateResponseWithOperator2 =>
            new EstateResponse
            {
                EstateName = TestData.EstateName,
                EstateId = TestData.EstateId,
                Operators = new List<EstateOperatorResponse>
                            {
                                new EstateOperatorResponse
                                {
                                    Name = TestData.OperatorIdentifier2,
                                    OperatorId = TestData.OperatorId,
                                    RequireCustomMerchantNumber = TestData.RequireCustomMerchantNumber,
                                    RequireCustomTerminalNumber = TestData.RequireCustomTerminalNumber
                                }
                            }
            };

        public static Guid VoucherId = Guid.Parse("C33DD3E1-E13F-4836-AC97-5B77F4269839");

        public static String Message = "Test Message";
        public static DateTime ExpiryDate = new DateTime(2020,12,5);

        public static String VoucherCode = "ABCDEF123456";

        public static String OperatorIdentifier = "Operator 1";

        public static Guid EstateId = Guid.Parse("492CF186-B118-48F5-B0ED-654D61E5BAEB");
        public static Guid TransactionId=Guid.Parse("7D565958-FA09-40DF-B743-BEC23152E93F");

        public static Decimal Value = 10.00m;

        public static String RecipientEmail = "testrecipient@hotmail.co.uk";

        public static String RecipientMobile = "123456789";

        public static DateTime IssuedDateTime = new DateTime(2020, 11, 5);

        public static IssueVoucherRequest IssueVoucherRequest = IssueVoucherRequest.Create(TestData.VoucherId,
                                                                                           TestData.OperatorIdentifier,
                                                                                           TestData.EstateId,
                                                                                           TestData.TransactionId,
                                                                                           TestData.IssuedDateTime,
                                                                                           TestData.Value,
                                                                                           TestData.RecipientEmail,
                                                                                           TestData.RecipientMobile);

        public static String Barcode = "1234567890";

        public static VoucherIssuedEvent VoucherIssuedEvent = VoucherIssuedEvent.Create(TestData.VoucherId,
                                                                                        TestData.EstateId,
                                                                                        TestData.IssuedDateTime,
                                                                                        TestData.RecipientEmail,
                                                                                        TestData.RecipientMobile);

        public static Guid ContractId = Guid.Parse("6F5C4480-7195-4CA0-870E-74851838716F");

        public static IssueVoucherResponse IssueVoucherResponse =>
            new IssueVoucherResponse
            {
                ExpiryDate = TestData.ExpiryDate,
                Message = TestData.Message,
                VoucherCode = TestData.VoucherCode,
                VoucherId = TestData.VoucherId
            };

        public static VoucherAggregate GetVoucherAggregateWithRecipientEmail()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
            aggregate.AddBarcode(TestData.Barcode);
            aggregate.Issue(TestData.RecipientEmail, null);

            return aggregate;
        }

        public static VoucherAggregate GetVoucherAggregateWithRecipientMobile()
        {
            VoucherAggregate aggregate = VoucherAggregate.Create(TestData.VoucherId);
            aggregate.Generate(TestData.OperatorIdentifier, TestData.EstateId, TestData.TransactionId, TestData.IssuedDateTime, TestData.Value);
            aggregate.AddBarcode(TestData.Barcode);
            aggregate.Issue(null, TestData.RecipientMobile);

            return aggregate;
        }
    }
}
