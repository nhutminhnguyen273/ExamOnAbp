using ExamOnAbp.Shared.Localization;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace ExamOnAbp.Shared.Hosting.AspNetCore
{
    [DependsOn(
        typeof(ExamOnAbpSharedHostingModule),
        typeof(ExamOnAbpSharedLocalizationModule), 
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
    )]
    public class ExamOnAbpSharedHostingAspNetCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ExamOnAbpSharedHostingAspNetCoreModule>("ExamOnAbp.Shared.Hosting.AspNetCore");
            });
        }
    }
}
