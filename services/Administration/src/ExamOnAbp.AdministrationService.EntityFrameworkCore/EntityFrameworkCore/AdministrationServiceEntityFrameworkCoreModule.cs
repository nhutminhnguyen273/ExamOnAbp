﻿using ExamOnAbp.AdministrationService.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace ExamOnAbp.AdministrationService.EntityFrameworkCore.EntityFrameworkCore
{
    [DependsOn(
        typeof(AdministrationServiceDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule)
    )]
    public class AdministrationServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AdministrationServiceDbContext>(options =>
            {
                options.ReplaceDbContext<IPermissionManagementDbContext>();
                options.ReplaceDbContext<ISettingManagementDbContext>();
                options.ReplaceDbContext<IAuditLoggingDbContext>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
            AppContext.SetSwitch("Npqsql.EnableLegacyTimestampBehavior", true);

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure<AdministrationServiceDbContext>(c =>
                {
                    c.UseNpgsql(b =>
                    {
                        b.MigrationsHistoryTable("__AdministrationService_Migrations");
                    });
                });
            });
        }
    }
}
