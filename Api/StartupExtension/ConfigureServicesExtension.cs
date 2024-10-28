using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.StartupExtension
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddDbContext<AuthDbContext>(options 
                => options.UseSqlServer(config.GetConnectionString("AuthConnection")));

            return services;
        }
    }
}
