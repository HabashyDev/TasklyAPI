using Microsoft.EntityFrameworkCore;
using Taskly.Core;
using Taskly.Core.Models;
using Taskly.Core.Repositories;
using Taskly.EF;
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
