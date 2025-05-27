using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using Taskly.Core;
using Taskly.Core.Models;
using Taskly.EF;
using TasklyAPI.Extensions;
namespace TasklyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddIdentity<AppUser,IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>();

            builder.Services.ConfigureAuthentication(builder.Configuration);

            builder.Services.AddAuthorization();

            builder.Services.AddSingleton<TokenProvider, TokenProvider>();


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.ConfigureDbContext(builder.Configuration);

            builder.Services.ConfigureCors();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();


            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log- .txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();


            app.UseSwagger(opt =>
            {
                opt.RouteTemplate = "openapi/{documentName}.json";
            });

            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Scalar Example";
                opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);

            });



            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors("CORSPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
