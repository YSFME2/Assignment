using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure
{
    public static class InfrastructureContainer
    {
        public static IServiceCollection AddInfrastructureServices(this  IServiceCollection services,IConfigurationManager configuration)
        {
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddDbContext<IAppDbContext, AppDbContext>((serviceProvider,options) => 
            {
                options.AddInterceptors(serviceProvider.GetServices<AuditableEntityInterceptor>());

                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //using (var dbContext = services.BuildServiceProvider().GetService<AppDbContext>())
            //{
            //    if (dbContext.Database.GetPendingMigrations().Any())
            //        dbContext.Database.Migrate();
            //}

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
