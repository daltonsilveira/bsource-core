using BSourceCore.API.Extensions;
using BSourceCore.API.Middleware;
using BSourceCore.Application;
using BSourceCore.Infrastructure;
using BSourceCore.Shared.Abstractions;
using Serilog;
using Serilog.Debugging;

SelfLog.Enable(Console.Error);

// Configure Serilog from separate configuration file
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("serilog.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"serilog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                     optional: true, reloadOnChange: true)
        .Build())
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting BSourceCore API");

    var builder = WebApplication.CreateBuilder(args);

    // Logging
    builder.AddSerilog();

    // Layer registration
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // API configuration
    builder.Services.AddControllers();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddPolicyAuthorization();
    builder.Services.AddApiVersioningDefaults();
    builder.Services.AddSwaggerDefaults();
    builder.Services.AddCorsDefaults();

    // Error handling
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // Apply migrations on startup
    using (var scope = app.Services.CreateScope())
    {
        var migrationService = scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
        try
        {
            await migrationService.ApplyMigrationsAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application startup failed due to migration error");
            throw;
        }
    }

    // Middleware pipeline
    app.UseExceptionHandler();
    app.UseSerilogDefaults();
    app.UseSwaggerDefaults();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
