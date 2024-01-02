using Web.Api.Services;
namespace Web.Api
{
    public static class WebApiContainer
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services,IConfigurationManager configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
