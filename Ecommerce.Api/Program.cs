using System.Text.Json.Serialization;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigureService(builder);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();

void ConfigureService(WebApplicationBuilder builder)
{
    var conn = builder.Configuration.GetConnectionString("SqlConnection");
    builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseSqlServer(conn));
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); 
}