using ExamOnAbp.AdministrationService.EntityFrameworkCore;
using ExamOnAbp.AdministrationService.EntityFrameworkCore.EntityFrameworkCore;
using ExamOnAbp.Shared.Hosting.AspNetCore;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using StackExchange.Redis;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;

namespace ExamOnAbp.Shared.Hosting.Microservices
{
    [DependsOn(
        typeof(ExamOnAbpSharedHostingAspNetCoreModule),
        typeof(AbpBackgroundJobsRabbitMqModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AdministrationServiceEntityFrameworkCoreModule),
        typeof(AbpDistributedLockingModule)
    )]
    public class ExamOnAbpSharedHostingMicroservicesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            IdentityModelEventSource.ShowPII = true;
            var configuration = context.Services.GetConfiguration();

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "ExamOnAbp:";
            });

            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            context.Services
                .AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "ExamOnAbp-Protection-Keys");

            context.Services.AddSingleton<IDistributedLockProvider>(sp =>
            {
                var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
                return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
            });
        }
    }
}
