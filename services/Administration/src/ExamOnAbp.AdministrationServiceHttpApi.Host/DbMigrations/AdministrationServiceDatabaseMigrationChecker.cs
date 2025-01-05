﻿using ExamOnAbp.AdministrationService.Domain;
using ExamOnAbp.AdministrationService.EntityFrameworkCore.EntityFrameworkCore;
using ExamOnAbp.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace ExamOnAbp.AdministrationServiceHttpApi.Host.DbMigrations
{
    public class AdministrationServiceDatabaseMigrationChecker
        : PendingEfCoreMigrationsChecker<AdministrationServiceDbContext>
    {
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public AdministrationServiceDatabaseMigrationChecker(
            IUnitOfWorkManager unitOfWorkManager,
            IServiceProvider serviceProvider,
            ICurrentTenant currentTenant,
            IDistributedEventBus distributedEventBus,
            IAbpDistributedLock abpDistributedLock,
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionDataSeeder permissionDataSeeder) 
            : base(
                  unitOfWorkManager,
                  serviceProvider,
                  currentTenant,
                  distributedEventBus,
                  abpDistributedLock,
                  AdministrationServiceDbProperties.ConnectionStringName)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
            _permissionDataSeeder = permissionDataSeeder;
        }

        public override async Task CheckAndApplyDatabaseMigrationsAsync()
        {
            await base.CheckAndApplyDatabaseMigrationsAsync();

            await TryAsync(async () => await SeedDataAsync());
        }

        private async Task SeedDataAsync()
        {
            using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true);
            var multiTenacySide = MultiTenancySides.Host;

            var permissionNames = (await _permissionDefinitionManager
                .GetPermissionsAsync())
                .Where(p => p.MultiTenancySide.HasFlag(multiTenacySide))
                .Where(p => !p.Providers.Any() ||
                            p.Providers.Contains(RolePermissionValueProvider.ProviderName))
                .Select(p => p.Name)
                .ToArray();

            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "admin",
                permissionNames
            );

            await uow.CompleteAsync();
        }
    }
}
