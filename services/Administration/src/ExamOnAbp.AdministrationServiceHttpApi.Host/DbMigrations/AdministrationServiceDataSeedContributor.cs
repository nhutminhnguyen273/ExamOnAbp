using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace ExamOnAbp.AdministrationServiceHttpApi.Host.DbMigrations
{
    public class AdministrationServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        protected const string IdentityUsersDefaultPermission = "AbpIdentity.Users";
        protected const string IdentityUsersLookupPermission = "AbpIdentity.UserLookup";

        public AdministrationServiceDataSeedContributor(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedClientCredentialPermissionsAsync();
        }

        private async Task SeedClientCredentialPermissionsAsync()
        {
        }
    }
}
