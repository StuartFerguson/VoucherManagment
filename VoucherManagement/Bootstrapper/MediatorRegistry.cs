namespace VoucherManagement.Bootstrapper;

using BusinessLogic.RequestHandlers;
using BusinessLogic.Requests;
using Lamar;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Models;

public class MediatorRegistry : ServiceRegistry
{
    public MediatorRegistry()
    {
        this.AddTransient<IMediator, Mediator>();

        // request & notification handlers
        this.AddTransient<ServiceFactory>(context =>
                                          {
                                              return t => context.GetService(t);
                                          });

        this.AddSingleton<IRequestHandler<IssueVoucherRequest, IssueVoucherResponse>, VoucherManagementRequestHandler>();
        this.AddSingleton<IRequestHandler<RedeemVoucherRequest, RedeemVoucherResponse>, VoucherManagementRequestHandler>();
    }
}