namespace VoucherManagement.Bootstrapper;

using System.IO.Abstractions;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

public class MiscRegistry : ServiceRegistry
{
    public MiscRegistry()
    {
        this.AddSingleton<Factories.IModelFactory, Factories.ModelFactory>();
        this.AddSingleton<IFileSystem, FileSystem>();
    }
}