using BankingSystem.Data;
using FluentValidation;
using MediatR;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Reflection;

namespace BankingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Host.UseSerilog((ctx, lc) => 
                lc.ReadFrom.Configuration(ctx.Configuration));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Banking System API",
                    Description = "Restful apis for the Banking System"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // add services to DI container
            {
                var services = builder.Services;
                services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
                services.AddDbContext<BankingContext>();

                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()))
                    .AddBehaviors();

                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

                services.AddExceptionHandling();
                services.AddCustomerBankAccountServices();
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseExceptionHandling();

            app.UseSwagger(c => {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankingSystem API V1");
            });

            app.UseSerilogRequestLogging();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}