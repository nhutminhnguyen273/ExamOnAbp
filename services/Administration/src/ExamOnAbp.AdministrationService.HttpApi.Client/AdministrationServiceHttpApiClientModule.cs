using ExamOnAbp.AdministrationService.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ExamOnAbp.AdministrationService.HttpApi.Client
{
    public class AdministrationServiceHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AdministrationServiceApplicationContractsModule).Assembly,
                AdministrationServiceRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
