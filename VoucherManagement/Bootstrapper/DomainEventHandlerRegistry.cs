namespace VoucherManagement.Bootstrapper;

using System;
using System.Collections.Generic;
using BusinessLogic.EventHandling;
using Lamar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.EventStore.EventHandling;

public class DomainEventHandlerRegistry : ServiceRegistry
{
    public DomainEventHandlerRegistry()
    {
        Dictionary<String, String[]> eventHandlersConfiguration = new Dictionary<String, String[]>();

        if (Startup.Configuration != null)
        {
            IConfigurationSection section = Startup.Configuration.GetSection("AppSettings:EventHandlerConfiguration");

            if (section != null)
            {
                Startup.Configuration.GetSection("AppSettings:EventHandlerConfiguration").Bind(eventHandlersConfiguration);
            }
        }
        this.AddSingleton<Dictionary<String, String[]>>(eventHandlersConfiguration);

        this.AddSingleton<Func<Type, IDomainEventHandler>>(container => (type) =>
                                                                        {
                                                                            IDomainEventHandler handler = container.GetService(type) as IDomainEventHandler;
                                                                            return handler;
                                                                        });

        this.AddSingleton<VoucherDomainEventHandler>();
        this.AddSingleton<IDomainEventHandlerResolver, DomainEventHandlerResolver>();
    }
}