namespace VoucherManagement.Bootstrapper;

using System;
using System.Net.Http;
using System.Net.Security;
using BusinessLogic;
using Common;
using EstateReporting.Database;
using Lamar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.DomainDrivenDesign.EventSourcing;
using Shared.EntityFramework;
using Shared.EntityFramework.ConnectionStringConfiguration;
using Shared.EventStore.Aggregate;
using Shared.EventStore.EventStore;
using Shared.EventStore.Extensions;
using Shared.General;
using Shared.Repositories;

public class RepositoryRegistry : ServiceRegistry
{
    public RepositoryRegistry()
    {
        Boolean useConnectionStringConfig = Boolean.Parse(ConfigurationReader.GetValue("AppSettings", "UseConnectionStringConfig"));

        if (useConnectionStringConfig)
        {
            String connectionStringConfigurationConnString = ConfigurationReader.GetConnectionString("ConnectionStringConfiguration");
            this.AddSingleton<IConnectionStringConfigurationRepository, ConnectionStringConfigurationRepository>();
            this.AddTransient<ConnectionStringConfigurationContext>(c =>
                                                                    {
                                                                        return new ConnectionStringConfigurationContext(connectionStringConfigurationConnString);
                                                                    });

            // TODO: Read this from a the database and set
        }
        else
        {
            Boolean insecureES = Startup.Configuration.GetValue<Boolean>("EventStoreSettings:Insecure");

            Func<SocketsHttpHandler> CreateHttpMessageHandler = () => new SocketsHttpHandler
                                                                      {
                                                                          SslOptions = new SslClientAuthenticationOptions
                                                                                       {
                                                                                           RemoteCertificateValidationCallback = (sender,
                                                                                               certificate,
                                                                                               chain,
                                                                                               errors) => {
                                                                                               return true;
                                                                                           }
                                                                                       }
                                                                      };

            this.AddEventStoreProjectionManagerClient(Startup.ConfigureEventStoreSettings);
            this.AddEventStorePersistentSubscriptionsClient(Startup.ConfigureEventStoreSettings);

            if (insecureES)
            {
                this.AddInSecureEventStoreClient(Startup.EventStoreClientSettings.ConnectivitySettings.Address, CreateHttpMessageHandler);
            }
            else
            {
                this.AddEventStoreClient(Startup.EventStoreClientSettings.ConnectivitySettings.Address, CreateHttpMessageHandler);
            }

            this.AddSingleton<IConnectionStringConfigurationRepository, ConfigurationReaderConnectionStringRepository>();
        }

        this.AddSingleton<Func<String, EstateReportingGenericContext>>(cont => (connectionString) =>
                                                                               {
                                                                                   String databaseEngine =
                                                                                       ConfigurationReader.GetValue("AppSettings", "DatabaseEngine");

                                                                                   return databaseEngine switch
                                                                                   {
                                                                                       "MySql" => new EstateReportingMySqlContext(connectionString),
                                                                                       "SqlServer" => new EstateReportingSqlServerContext(connectionString),
                                                                                       _ => throw new
                                                                                           NotSupportedException($"Unsupported Database Engine {databaseEngine}")
                                                                                   };
                                                                               });

        this.AddTransient<IEventStoreContext, EventStoreContext>();
        this.AddSingleton<IAggregateRepository<VoucherAggregate.VoucherAggregate, DomainEvent>, AggregateRepository<VoucherAggregate.VoucherAggregate, DomainEvent>>();

        this.AddSingleton<IDbContextFactory<EstateReportingGenericContext>, DbContextFactory<EstateReportingGenericContext>>();
    }
}