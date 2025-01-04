using ExamOnAbp.Shared.Localization.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace ExamOnAbp.Shared.Localization
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class ExamOnAbpSharedLocalizationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ExamOnAbpSharedLocalizationModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ExamOnAbpResource>("vi")
                    .AddBaseTypes(
                        typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/ExamOnAbp");
                options.DefaultResourceType = typeof(ExamOnAbpResource);
            });
        }
    }
}