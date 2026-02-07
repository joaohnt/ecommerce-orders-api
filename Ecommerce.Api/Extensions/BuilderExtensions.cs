using System.Text.Json.Serialization;
using Ecommerce.Api.ExceptionHandling;
using Ecommerce.Application.Repositories;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.Consumer;
using Ecommerce.Infrastructure.Database.Context;
using Ecommerce.Infrastructure.Mongo;
using Ecommerce.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;

namespace Ecommerce.Api.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); 
        builder.Services.AddOpenApi();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Host.UseSerilog();
        return builder;
    }
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddTransient<IOrderRepository, OrderRepository>();
        return builder;
    }
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        var conn = builder.Configuration.GetConnectionString("SqlConnection");
        builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseSqlServer(conn));
        
        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
        builder.Services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = builder.Configuration
                .GetSection("MongoDb")
                .Get<MongoDbSettings>();
            return new MongoClient(settings!.ConnectionString);
        });
        return builder;
    }

    public static WebApplicationBuilder AddMessaging(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    builder.Configuration["RabbitMq:Host"],
                    builder.Configuration["RabbitMq:VirtualHost"],
                    h =>
                    {
                        h.Username(builder.Configuration["RabbitMq:Username"]);
                        h.Password(builder.Configuration["RabbitMq:Password"]);
                    });        cfg.ReceiveEndpoint("meu-evento", e =>
                {
                    e.ConfigureConsumer<OrderCreatedConsumer>(context);
                });
            });
        });
        return builder;
    }
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        var outputTemplate =
            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] " +
            "{SourceContext} | {Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("MassTransit", LogEventLevel.Warning)
            .MinimumLevel.Override("MassTransit.Messages", LogEventLevel.Warning)
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.MongoDB(
                "mongodb://admin:admin123@localhost:27017/EcommerceLogs?authSource=admin",
                collectionName: "EcommerceLogs")
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
    
}