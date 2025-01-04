using Volo.Abp.Autofac; // Sử dụng Autofac làm Dependency Injection container
using Volo.Abp.Data;
using Volo.Abp.Modularity; // Dùng định nghĩa các module trong hệ thống ABP

namespace ExamOnAbp.Shared.Hosting
{
    [DependsOn(
        typeof(AbpAutofacModule), // Module cho phép tích hợp với Autofac
        typeof(AbpDataModule) // Module cung cấp các tính năng quản lý dữ liệu, kết nối với cơ sở dữ liệu
    )]
    public class ExamOnAbpSharedHostingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureDatabaseConnections();
        }

        private void ConfigureDatabaseConnections()
        {
            Configure<AbpDbConnectionOptions>(options =>
            {
                options.Databases.Configure("AdministrationService", database =>
                {
                    database.MappedConnections.Add("AbpAudtiLogging");
                    database.MappedConnections.Add("AbpPermissionManagement");
                    database.MappedConnections.Add("AbpSettingManagement");
                });

                options.Databases.Configure("IdentityService", database =>
                {
                    database.MappedConnections.Add("AbpIdentity");
                    database.MappedConnections.Add("AbpIdentityServer");
                });
            });
        }
    }
}
