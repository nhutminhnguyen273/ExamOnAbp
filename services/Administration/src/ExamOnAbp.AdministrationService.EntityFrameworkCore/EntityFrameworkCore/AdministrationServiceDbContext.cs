using ExamOnAbp.AdministrationService.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace ExamOnAbp.AdministrationService.EntityFrameworkCore.EntityFrameworkCore
{
    [ConnectionStringName(AdministrationServiceDbProperties.ConnectionStringName)]
    public class AdministrationServiceDbContext 
        : AbpDbContext<AdministrationServiceDbContext>,
        IPermissionManagementDbContext,
        ISettingManagementDbContext,
        IAuditLoggingDbContext
    {
        public AdministrationServiceDbContext(DbContextOptions<AdministrationServiceDbContext> options)
            : base(options) { }

        #region DbSet
        // Permission Management
        public DbSet<PermissionGroupDefinitionRecord> PermissionGroups => throw new NotImplementedException();

        public DbSet<PermissionDefinitionRecord> Permissions => throw new NotImplementedException();

        public DbSet<PermissionGrant> PermissionGrants => throw new NotImplementedException();

        // Setting Management
        public DbSet<Setting> Settings => throw new NotImplementedException();

        public DbSet<SettingDefinitionRecord> SettingDefinitionRecords => throw new NotImplementedException();

        // Audit Logging
        public DbSet<AuditLog> AuditLogs => throw new NotImplementedException();
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePermissionManagement();
            modelBuilder.ConfigureSettingManagement();
            modelBuilder.ConfigureAuditLogging();
        }
    }
}
