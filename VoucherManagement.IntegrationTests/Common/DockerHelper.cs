using System;
using System.Collections.Generic;
using System.Text;

namespace VoucherManagement.IntegrationTests.Common
{
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using Ductus.FluentDocker.Builders;
    using Ductus.FluentDocker.Common;
    using Ductus.FluentDocker.Model.Builders;
    using Ductus.FluentDocker.Services;
    using Ductus.FluentDocker.Services.Extensions;
    using EstateManagement.Client;
    using EstateReporting.Database;
    using EventStore.Client;
    using global::Shared.Logger;
    using Microsoft.Data.SqlClient;
    using SecurityService.Client;
    
    public class DockerHelper : global::Shared.IntegrationTesting.DockerHelper
    {
        #region Fields

        /// <summary>
        /// The estate client
        /// </summary>
        public IEstateClient EstateClient;

        /// <summary>
        /// The security service client
        /// </summary>
        public ISecurityServiceClient SecurityServiceClient;

        /// <summary>
        /// The test identifier
        /// </summary>
        public Guid TestId;

        /// <summary>
        /// The voucher management client
        /// </summary>
        public IVoucherManagementClient VoucherManagementClient;

        /// <summary>
        /// The containers
        /// </summary>
        protected List<IContainerService> Containers;

        /// <summary>
        /// The estate management API port
        /// </summary>
        protected Int32 EstateManagementApiPort;

        /// <summary>
        /// The event store HTTP port
        /// </summary>
        protected Int32 EventStoreHttpPort;

        /// <summary>
        /// The security service port
        /// </summary>
        protected Int32 SecurityServicePort;

        /// <summary>
        /// The test networks
        /// </summary>
        protected List<INetworkService> TestNetworks;

        private readonly TestingContext TestingContext;

        private Int32 VoucherManagementPort;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerHelper" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="testingContext">The testing context.</param>
        public DockerHelper(NlogLogger logger, TestingContext testingContext)
        {
            this.Logger = logger;
            this.TestingContext = testingContext;
            this.Containers = new List<IContainerService>();
            this.TestNetworks = new List<INetworkService>();
        }

        #endregion

        /// <summary>
        /// Populates the subscription service configuration.
        /// </summary>
        /// <param name="estateName">Name of the estate.</param>
        public async Task PopulateSubscriptionServiceConfiguration(String estateName)
        {
            var name = estateName.Replace(" ", "");
            List<(string streamName, string groupName)> subscriptions = new List<(String, String)>();
            subscriptions.Add((name, "Reporting"));
            subscriptions.Add(($"EstateManagementSubscriptionStream_{name}", "Estate Management"));
            await this.PopulateSubscriptionServiceConfiguration(this.EventStoreHttpPort, subscriptions);
        }
        
        #region Methods

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public override async Task StartContainersForScenarioRun(String scenarioName)
        {
            this.HostTraceFolder = FdOs.IsWindows() ? $"D:\\home\\txnproc\\trace\\{scenarioName}" : $"//home//txnproc//trace//{scenarioName}";

            this.SqlServerDetails = (Setup.SqlServerContainerName, Setup.SqlUserName, Setup.SqlPassword);

            this.ClientDetails = ("serviceClient", "Secret1");
            Logging.Enabled();

            Guid testGuid = Guid.NewGuid();
            this.TestId = testGuid;

            this.Logger.LogInformation($"Test Id is {testGuid}");

            // Setup the container names
            this.SecurityServiceContainerName = $"securityservice{testGuid:N}";
            this.EstateManagementContainerName = $"estate{testGuid:N}";
            this.EventStoreContainerName = $"eventstore{testGuid:N}";
            this.EstateReportingContainerName = $"estatereporting{testGuid:N}";
            this.VoucherManagementContainerName = $"vouchermanagement{testGuid:N}";

            this.DockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

            INetworkService testNetwork = DockerHelper.SetupTestNetwork();
            this.TestNetworks.Add(testNetwork);
            IContainerService eventStoreContainer = this.SetupEventStoreContainer("eventstore/eventstore:21.10.0-buster-slim", testNetwork);
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint($"{DockerHelper.EventStoreHttpDockerPort}/tcp").Port;

            String insecureEventStoreEnvironmentVariable = "EventStoreSettings:Insecure=true";
            String persistentSubscriptionPollingInSeconds = "AppSettings:PersistentSubscriptionPollingInSeconds=10";
            String internalSubscriptionServiceCacheDuration = "AppSettings:InternalSubscriptionServiceCacheDuration=0";
            
            IContainerService estateManagementContainer = this.SetupEstateManagementContainer("stuartferguson/estatemanagement", new List<INetworkService>
                                                                                                  {
                                                                                                      testNetwork,
                                                                                                      Setup.DatabaseServerNetwork
                                                                                                  },
                                                                                              true,
                                                                                              additionalEnvironmentVariables: new List<String>
                                                                                                  {
                                                                                                      insecureEventStoreEnvironmentVariable,
                                                                                                      internalSubscriptionServiceCacheDuration,
                                                                                                      persistentSubscriptionPollingInSeconds
                                                                                                  });

            IContainerService securityServiceContainer = this.SetupSecurityServiceContainer("stuartferguson/securityservice",
                                                                                                    testNetwork,
                                                                                                    true);

            IContainerService voucherManagementContainer = this.SetupVoucherManagementContainer("vouchermanagement",
                                                                                                              new List<INetworkService>
                                                                                                              {
                                                                                                                  testNetwork,
                                                                                                                  Setup.DatabaseServerNetwork
                                                                                                              }, additionalEnvironmentVariables: new List<String>
                                                                                                                  {
                                                                                                                      insecureEventStoreEnvironmentVariable,
                                                                                                                      internalSubscriptionServiceCacheDuration,
                                                                                                                      persistentSubscriptionPollingInSeconds
                                                                                                                  });

            IContainerService estateReportingContainer = this.SetupEstateReportingContainer("stuartferguson/estatereporting",
                                                                                            new List<INetworkService>
                                                                                            {
                                                                                                testNetwork,
                                                                                                Setup.DatabaseServerNetwork,
                                                                                            },
                                                                                            true,
                                                                                            additionalEnvironmentVariables: new List<String>
                                                                                                {
                                                                                                    insecureEventStoreEnvironmentVariable,
                                                                                                    internalSubscriptionServiceCacheDuration,
                                                                                                    persistentSubscriptionPollingInSeconds
                                                                                                });
            
            this.Containers.AddRange(new List<IContainerService>
                                     {
                                         eventStoreContainer,
                                         estateManagementContainer,
                                         securityServiceContainer,
                                         voucherManagementContainer,
                                         estateReportingContainer,
                                     });

            // Cache the ports
            this.EstateManagementApiPort = estateManagementContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.SecurityServicePort = securityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;
            this.EventStoreHttpPort = eventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;
            this.VoucherManagementPort = voucherManagementContainer.ToHostExposedEndpoint("5007/tcp").Port;

            // Setup the base address resolvers
            String EstateManagementBaseAddressResolver(String api) => $"http://127.0.0.1:{this.EstateManagementApiPort}";
            String SecurityServiceBaseAddressResolver(String api) => $"https://127.0.0.1:{this.SecurityServicePort}";
            String VoucherManagementBaseAddressResolver(String api) => $"http://127.0.0.1:{this.VoucherManagementPort}";

            HttpClientHandler clientHandler = new HttpClientHandler
                                              {
                                                  ServerCertificateCustomValidationCallback = (message,
                                                                                               certificate2,
                                                                                               arg3,
                                                                                               arg4) =>
                                                                                              {
                                                                                                  return true;
                                                                                              }

                                              };
            HttpClient httpClient = new HttpClient(clientHandler);
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient);
            this.SecurityServiceClient = new SecurityServiceClient(SecurityServiceBaseAddressResolver, httpClient);
            this.VoucherManagementClient = new VoucherManagementClient(VoucherManagementBaseAddressResolver, httpClient);

            await this.LoadEventStoreProjections(this.EventStoreHttpPort).ConfigureAwait(false);
        }
        
        private async Task RemoveEstateReadModel()
        {
            List<Guid> estateIdList = this.TestingContext.GetAllEstateIds();

            foreach (Guid estateId in estateIdList)
            {
                String databaseName = $"EstateReportingReadModel{estateId}";

                await Retry.For(async () =>
                {
                    // Build the connection string (to master)
                    String connectionString = Setup.GetLocalConnectionString(databaseName);
                    EstateReportingSqlServerContext context = new EstateReportingSqlServerContext(connectionString);
                    await context.Database.EnsureDeletedAsync(CancellationToken.None);
                });
            }
        }
        
        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun()
        {
            await RemoveEstateReadModel().ConfigureAwait(false);

            if (this.Containers.Any())
            {
                foreach (IContainerService containerService in this.Containers)
                {
                    containerService.StopOnDispose = true;
                    containerService.RemoveOnDispose = true;
                    containerService.Dispose();
                }
            }

            if (this.TestNetworks.Any())
            {
                foreach (INetworkService networkService in this.TestNetworks)
                {
                    networkService.Stop();
                    networkService.Remove(true);
                }
            }
        }

        #endregion
    }
}
