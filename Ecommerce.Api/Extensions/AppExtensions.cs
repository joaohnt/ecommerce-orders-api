using Ecommerce.Infrastructure.Database.Context;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Extensions;

public static class AppExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
        db.Database.Migrate();
        return app;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        app.MapControllers();
        app.UseExceptionHandler();
        if (app.Environment.IsDevelopment())
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
            });
        }
        else
        {
            app.UseHangfireDashboard("/hangfire");
        }
        app.UseSwagger();
        app.UseSwaggerUI();
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }
        return app;
    }

    private sealed class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
