namespace VoucherManagement.Bootstrapper;

using System;
using System.Net.Http;
using EstateManagement.Client;
using Lamar;
using MessagingService.Client;
using Microsoft.Extensions.DependencyInjection;
using SecurityService.Client;
using Shared.General;

public class ClientRegistry : ServiceRegistry
{
    public ClientRegistry()
    {
        this.AddSingleton<Func<String, String>>(container => (serviceName) =>
                                                             {
                                                                 return ConfigurationReader.GetBaseServerUri(serviceName).OriginalString;
                                                             });

        HttpClientHandler httpClientHandler = new HttpClientHandler
                                              {
                                                  ServerCertificateCustomValidationCallback = (message,
                                                                                               certificate2,
                                                                                               arg3,
                                                                                               arg4) =>
                                                                                              {
                                                                                                  return true;
                                                                                              }
                                              };
        HttpClient httpClient = new HttpClient(httpClientHandler);
        this.AddSingleton<HttpClient>(httpClient);
        this.AddSingleton<IEstateClient, EstateClient>();
        this.AddSingleton<ISecurityServiceClient, SecurityServiceClient>();
        this.AddSingleton<IMessagingServiceClient, MessagingServiceClient>();
    }
}