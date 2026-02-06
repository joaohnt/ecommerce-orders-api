using Ecommerce.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddApi()
    .AddApplicationServices()
    .AddDatabase()
    .AddMessaging();

var app = builder.Build();
app.UseApi();

app.Run();
