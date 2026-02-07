using Ecommerce.Application.Repositories;
using Ecommerce.Application.DTOs;
using Ecommerce.Infrastructure.Consumer;
using Ecommerce.Infrastructure.Database.Context;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Worker;
using Ecommerce.Worker.BackgroundJobs;
using Hangfire;
using Hangfire.SqlServer;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlConnection"),
        sql => sql.EnableRetryOnFailure()));

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<ProcessOrderJob>();
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("SqlConnection"),
        new SqlServerStorageOptions
        {
            PrepareSchemaIfNecessary = true,
            QueuePollInterval = TimeSpan.FromSeconds(5)
        });
});

builder.Services.AddHangfireServer(options =>
{
    options.ServerName = "worker-1";
    options.WorkerCount = Environment.ProcessorCount;
    options.Queues = new[] { "default" };
});

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
    .MinimumLevel.Override("Ecommerce.Worker.Worker", LogEventLevel.Warning) 
    .MinimumLevel.Override("Hangfire.Processing", LogEventLevel.Warning)
    .MinimumLevel.Override("Hangfire.Server", LogEventLevel.Warning)
    .MinimumLevel.Override("Hangfire.SqlServer", LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate: outputTemplate)
    .WriteTo.MongoDB(
        "mongodb://admin:admin123@localhost:27017/EcommerceLogs?authSource=admin",
        collectionName: "EcommerceLogs")
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Services.AddSerilog(Log.Logger);


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
            });

        cfg.ReceiveEndpoint("meu-evento", e =>
        {
            e.Bind<OrderCreatedPayload>();
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
    });
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
