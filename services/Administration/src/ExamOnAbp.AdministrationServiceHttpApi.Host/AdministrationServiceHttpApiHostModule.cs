using ExamOnAbp.AdministrationService.Application;
using ExamOnAbp.AdministrationService.EntityFrameworkCore.EntityFrameworkCore;
using ExamOnAbp.AdministrationService.HttpApi;
using ExamOnAbp.AdministrationServiceHttpApi.Host.DbMigrations;
using ExamOnAbp.Shared.Hosting.AspNetCore;
using ExamOnAbp.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Cors;
using Volo.Abp;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace ExamOnAbp.AdministrationServiceHttpApi.Host
{
    [DependsOn(
        typeof(AdministrationServiceHttpApiModule),
        typeof(AdministrationServiceApplicationModule),
        typeof(AdministrationServiceEntityFrameworkCoreModule),
        typeof(ExamOnAbpSharedHostingMicroservicesModule),
        typeof(AbpIdentityHttpApiClientModule)
    )]
    public class AdministrationServiceHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            JwtBearerConfigurationHelper.Configure(context, "AdministrationService");

            SwaggerConfigurationHelper.ConfigureWithOidc(
                context: context,
                authority: configuration["AuthServer:Authority"]!,
                scopes: ["AdministrationService"],
                discoveryEndpoint: configuration["AuthServer:MetadataAddress"],
                apiTitle: "Administration Service API"
            );

            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]!
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
                            .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            Configure<PermissionManagementOptions>(options => { options.IsDynamicPermissionStoreEnabled = true; });

            Configure<SettingManagementOptions>(options => { options.IsDynamicSettingStoreEnabled = true; });

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            app.UseCorrelationId();
            app.UseCors();
            app.UseAbpRequestLocalization();
            app.MapAbpStaticAssets();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAbpClaimsMap();
            app.UseUnitOfWork();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerWithCustomScriptUI(options =>
            {
                var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Administration Service API");
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            });
            app.UseAbpSerilogEnrichers();
            app.UseAuditing();
            app.UseConfiguredEndpoints();
        }

        public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            await context.ServiceProvider
                .GetRequiredService<AdministrationServiceDatabaseMigrationChecker>()
                .CheckAndApplyDatabaseMigrationsAsync();
        }
    }
}
