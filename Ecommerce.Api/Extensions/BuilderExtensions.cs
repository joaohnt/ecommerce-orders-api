using System.Text.Json.Serialization;
using Ecommerce.Api.ExceptionHandling;
using Ecommerce.Application.Repositories;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.Consumer;
using Ecommerce.Infrastructure.Database.Context;
using Ecommerce.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); 
        builder.Services.AddOpenApi();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
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
    
}