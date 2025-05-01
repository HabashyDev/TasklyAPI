using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
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

            // Add services to the container.



            builder.Services.AddControllers();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.ConfigureDbContext(builder.Configuration);

            builder.Services.ConfigureIdentity();

            builder.Services.ConfigureJWT(builder.Configuration);

            builder.Services.ConfigureCors();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();




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





            app.UseHttpsRedirection();

            app.UseCors("CORSPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
