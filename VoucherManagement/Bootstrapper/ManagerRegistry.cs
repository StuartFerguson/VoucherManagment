namespace VoucherManagement.Bootstrapper;

using BusinessLogic.Manager;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

public class ManagerRegistry : ServiceRegistry
{
    public ManagerRegistry()
    {
        this.AddSingleton<IVoucherManagementManager, VoucherManagementManager>();
    }
}