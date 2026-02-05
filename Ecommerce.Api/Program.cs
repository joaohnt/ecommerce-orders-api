using Ecommerce.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigureService(builder);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

void ConfigureService(WebApplicationBuilder builder)
{
    var conn = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseSqlServer(conn));
}