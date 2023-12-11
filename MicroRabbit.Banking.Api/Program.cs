using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(cfg =>
        {
            cfg.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Banking MicroService",
                Version = "v1"
            });
        });
        builder.Services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
        });

        builder.Services.AddDbContext<BankingDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("BankingDbConnection"));
        });

        RegisterServices(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking MicroService V1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        DependencyContainer.RegisterServices(services);
    }
}
