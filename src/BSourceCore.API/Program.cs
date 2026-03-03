using System.Diagnostics;
using System.Text;
using Asp.Versioning;
using BSourceCore.API.Middleware;
using BSourceCore.Application;
using BSourceCore.Infrastructure;
using BSourceCore.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

    // Use Serilog
    builder.Configuration
        .AddJsonFile("serilog.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"serilog.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
    
    builder.Logging.ClearProviders();
    
    builder.Host.UseSerilog((ctx, services, lc) => lc
            .ReadFrom.Configuration(ctx.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext(),
        preserveStaticLogger: true,
        writeToProviders: false);

    // Add services to the container.
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();

    // JWT Authentication
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"] ?? "BSourceCore-Super-Secret-Key-For-Development-Only-Must-Be-At-Least-32-Characters";

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"] ?? "BSourceCore",
            ValidAudience = jwtSettings["Audience"] ?? "BSourceCore.API",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        // Tenant policies
        options.AddPolicy("tenants.create", policy => policy.Requirements.Add(new PermissionRequirement("tenants.create")));
        options.AddPolicy("tenants.read", policy => policy.Requirements.Add(new PermissionRequirement("tenants.read")));
        options.AddPolicy("tenants.update", policy => policy.Requirements.Add(new PermissionRequirement("tenants.update")));
        options.AddPolicy("tenants.delete", policy => policy.Requirements.Add(new PermissionRequirement("tenants.delete")));

        // User policies
        options.AddPolicy("users.create", policy => policy.Requirements.Add(new PermissionRequirement("users.create")));
        options.AddPolicy("users.read", policy => policy.Requirements.Add(new PermissionRequirement("users.read")));
        options.AddPolicy("users.update", policy => policy.Requirements.Add(new PermissionRequirement("users.update")));
        options.AddPolicy("users.delete", policy => policy.Requirements.Add(new PermissionRequirement("users.delete")));

        // Group policies
        options.AddPolicy("groups.create", policy => policy.Requirements.Add(new PermissionRequirement("groups.create")));
        options.AddPolicy("groups.read", policy => policy.Requirements.Add(new PermissionRequirement("groups.read")));
        options.AddPolicy("groups.update", policy => policy.Requirements.Add(new PermissionRequirement("groups.update")));
        options.AddPolicy("groups.delete", policy => policy.Requirements.Add(new PermissionRequirement("groups.delete")));

        // Permission policies
        options.AddPolicy("permissions.create", policy => policy.Requirements.Add(new PermissionRequirement("permissions.create")));
        options.AddPolicy("permissions.read", policy => policy.Requirements.Add(new PermissionRequirement("permissions.read")));
        options.AddPolicy("permissions.update", policy => policy.Requirements.Add(new PermissionRequirement("permissions.update")));
        options.AddPolicy("permissions.delete", policy => policy.Requirements.Add(new PermissionRequirement("permissions.delete")));
    });

    // API Versioning
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "BSourceCore API",
            Version = "v1",
            Description = "BSource Management Platform API"
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
        });
    });

    // Global Exception Handling
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    

    // Configure the HTTP request pipeline.
    app.UseExceptionHandler();

    // Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.FirstOrDefault());
            diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString());
        };
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "BSourceCore API v1");
            options.RoutePrefix = string.Empty;
        });
    }

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
