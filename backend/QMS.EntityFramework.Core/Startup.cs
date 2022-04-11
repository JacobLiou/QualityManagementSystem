using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql;
using QMS.Core;

namespace QMS.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.CustomizeMultiTenants(); // 自定义租户

                options.AddDbPool<DefaultDbContext>(providerName: default, optionBuilder: opt =>
                {
                    opt.UseMySql(ServerVersion.Parse("5.7.34")); // EF批量组件
                });
                options.AddDbPool<MultiTenantDbContext, MultiTenantDbContextLocator>();
                options.AddDbPool<IssuesDbContext, IssuesDbContextLocator>(providerName: default, optionBuilder: opt =>
                {
                    opt.UseMySql(ServerVersion.Parse("5.7.34")); // EF批量组件
                }); ;

            }, "QMS.Database.Migrations");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 自动迁移数据库（update-database命令）
            if (env.IsDevelopment())
            {
                Scoped.Create((_, scope) =>
                {
                    var context = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
                    context.Database.Migrate();
                    //context.Database.EnsureCreated();
                });
                Scoped.Create((_, scope) =>
                {
                    var context = scope.ServiceProvider.GetRequiredService<MultiTenantDbContext>();
                    
                    //context.Database.EnsureCreated();
                    context.Database.Migrate();
                });


                Scoped.Create((_, scope) =>
                {
                    var context = scope.ServiceProvider.GetRequiredService<IssuesDbContext>();

                    //context.Database.EnsureCreated();
                    context.Database.Migrate();
                });
            }
        }
    }
}