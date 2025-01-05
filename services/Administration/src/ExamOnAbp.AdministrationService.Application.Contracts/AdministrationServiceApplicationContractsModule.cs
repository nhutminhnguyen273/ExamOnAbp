using ExamOnAbp.AdministrationService.Domain.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace ExamOnAbp.AdministrationService.Application.Contracts
{
    [DependsOn(
        typeof(AdministrationServiceDomainSharedModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule)
    )]
    public class AdministrationServiceApplicationContractsModule : AbpModule
    {
    }
}
