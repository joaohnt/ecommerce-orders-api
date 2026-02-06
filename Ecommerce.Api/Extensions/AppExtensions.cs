namespace Ecommerce.Api.Extensions;

public static class AppExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}