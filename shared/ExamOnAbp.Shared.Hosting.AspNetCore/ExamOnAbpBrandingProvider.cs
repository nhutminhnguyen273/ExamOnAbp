using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ExamOnAbp.Shared.Hosting.AspNetCore
{
    [Dependency(ReplaceServices = true)]
    public class ExamOnAbpBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "ExamOnAbp";
    }
}
