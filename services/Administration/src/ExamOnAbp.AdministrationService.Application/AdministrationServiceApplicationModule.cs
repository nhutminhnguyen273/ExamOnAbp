using ExamOnAbp.AdministrationService.Application.Contracts;
using ExamOnAbp.AdministrationService.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace ExamOnAbp.AdministrationService.Application
{
    [DependsOn(
        typeof(AdministrationServiceDomainModule),
        typeof(AdministrationServiceApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule)
    )]
    public class AdministrationServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AdministrationServiceApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AdministrationServiceApplicationModule>(validate: true);
            });
        }
    }
}
