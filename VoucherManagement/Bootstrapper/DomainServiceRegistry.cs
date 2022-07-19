namespace VoucherManagement.Bootstrapper;

using BusinessLogic.Services;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

public class DomainServiceRegistry : ServiceRegistry
{
    public DomainServiceRegistry()
    {
        this.AddSingleton<IVoucherDomainService, VoucherDomainService>();
    }
}