using ExamOnAbp.AdministrationService.Domain.Shared.Localization;
using Volo.Abp.Application.Services;

namespace ExamOnAbp.AdministrationService.Application
{
    public abstract class AdministrationServiceAppService : ApplicationService
    {
        protected AdministrationServiceAppService() 
        {
            LocalizationResource = typeof(AdministrationServiceResource);
        }
    }
}
