using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Taskly.Core;
using Taskly.Core.Models;
using Taskly.Core.Repositories;
using Taskly.EF;
using System.Text;
using Taskly.EF.Repositories;
namespace TasklyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.AddControllers();


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddDbContext<AppDBContext>(options => options
            .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
            , b => b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName)));

            var JwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();


            

            builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme
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
            builder.Services.AddSingleton(JwtOptions);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
