using ExamOnAbp.AdministrationService.Domain.Shared;
using Volo.Abp.AuditLogging;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;

namespace ExamOnAbp.AdministrationService.Domain
{
    [DependsOn(
        typeof(AdministrationServiceDomainSharedModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule)
    )]
    public class AdministrationServiceDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("vi", "vi", "Vietnamese"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });
        }
    }
}
