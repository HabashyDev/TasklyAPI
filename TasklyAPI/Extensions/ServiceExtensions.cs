using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Taskly.EF;
using Taskly.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;


namespace TasklyAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection service , IConfiguration configuration) 
        {
            service.AddDbContext<AppDBContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection")
            , b => b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName)));
        }

        public static void ConfigureCors(this IServiceCollection services) 
        {
            services.AddCors(options => {
                options.AddPolicy(name: "CORSPolicy", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod());
            });
        }

        public static void ConfigureJWT(this IServiceCollection service, IConfiguration configuration)
        {
            var JwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

            service.AddSingleton(JwtOptions);

            service.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme
                , options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = JwtOptions.Audiance,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey))
                    };
                });

        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>();
                
        }
    }
}
