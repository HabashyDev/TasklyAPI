using Microsoft.EntityFrameworkCore;
using Taskly.EF;


namespace TasklyAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<AppDBContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection")
            , b => b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName)));
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CORSPolicy", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod());
            });
        }

    }
}
