namespace Ecommerce.Api.Extensions;

public static class AppExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.MapControllers();
        app.UseExceptionHandler();
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}