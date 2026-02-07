using Ecommerce.Api.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.AddApi()
    .AddApplicationServices()
    .AddDatabase()
    .AddMessaging()
    .AddHangfireMonitoring()
    .AddSerilogLogging();

var app = builder.Build();
app.ApplyMigrations();
app.UseApi();
app.Run();
