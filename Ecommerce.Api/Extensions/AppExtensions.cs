using Hangfire;

namespace Ecommerce.Api.Extensions;

public static class AppExtensions
{
    public static WebApplication UseApi(this WebApplication app)
    {
        app.MapControllers();
        app.UseExceptionHandler();
        app.UseHangfireDashboard("/hangfire");
        app.UseSwagger();
        app.UseSwaggerUI();
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }
        return app;
    }
}
