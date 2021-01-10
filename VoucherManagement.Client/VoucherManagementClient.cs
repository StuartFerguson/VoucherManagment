namespace VoucherManagement.Client
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using DataTransferObjects;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClientProxyBase.ClientProxyBase" />
    /// <seealso cref="VoucherManagement.Client.IVoucherManagementClient" />
    public class VoucherManagementClient : ClientProxyBase, IVoucherManagementClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        private readonly String BaseAddress;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VoucherManagementClient" /> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public VoucherManagementClient(Func<String, String> baseAddressResolver,
                                       HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("VoucherManagementApi");

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        }

        /// <summary>
        /// Issues the voucher.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="issueVoucherRequest">The issue voucher request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<IssueVoucherResponse> IssueVoucher(String accessToken,
                                                             IssueVoucherRequest issueVoucherRequest,
                                                             CancellationToken cancellationToken)
        {
            IssueVoucherResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/vouchers";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(issueVoucherRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<IssueVoucherResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error issuing voucher.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the voucher.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="voucherCode">The voucher code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetVoucherResponse> GetVoucher(String accessToken,
                                                         Guid estateId,
                                                         String voucherCode,
                                                         CancellationToken cancellationToken)
        {
            GetVoucherResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/vouchers?estateId={estateId}&voucherCode={voucherCode}";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetVoucherResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting voucher.", ex);

                throw exception;
            }

            return response;
        }
    }
}