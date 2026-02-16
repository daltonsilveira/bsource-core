using System.Text;
using Asp.Versioning;
using BSourceCore.API.Middleware;
using BSourceCore.Application;
using BSourceCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

// Configure Serilog from separate configuration file
var serilogConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("serilog.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"serilog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(serilogConfiguration)
    .CreateLogger();

try
{
    Log.Information("Starting BSourceCore API");

var builder = WebApplication.CreateBuilder(args);

// Use Serilog
builder.Host.UseSerilog();

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
    options.AddPolicy("tenants.create", policy => policy.RequireClaim("permission", "tenants.create"));
    options.AddPolicy("tenants.read", policy => policy.RequireClaim("permission", "tenants.read"));
    options.AddPolicy("tenants.update", policy => policy.RequireClaim("permission", "tenants.update"));
    options.AddPolicy("tenants.delete", policy => policy.RequireClaim("permission", "tenants.delete"));

    // User policies
    options.AddPolicy("users.create", policy => policy.RequireClaim("permission", "users.create"));
    options.AddPolicy("users.read", policy => policy.RequireClaim("permission", "users.read"));
    options.AddPolicy("users.update", policy => policy.RequireClaim("permission", "users.update"));
    options.AddPolicy("users.delete", policy => policy.RequireClaim("permission", "users.delete"));

    // Group policies
    options.AddPolicy("groups.create", policy => policy.RequireClaim("permission", "groups.create"));
    options.AddPolicy("groups.read", policy => policy.RequireClaim("permission", "groups.read"));
    options.AddPolicy("groups.update", policy => policy.RequireClaim("permission", "groups.update"));
    options.AddPolicy("groups.delete", policy => policy.RequireClaim("permission", "groups.delete"));

    // Permission policies
    options.AddPolicy("permissions.create", policy => policy.RequireClaim("permission", "permissions.create"));
    options.AddPolicy("permissions.read", policy => policy.RequireClaim("permission", "permissions.read"));
    options.AddPolicy("permissions.update", policy => policy.RequireClaim("permission", "permissions.update"));
    options.AddPolicy("permissions.delete", policy => policy.RequireClaim("permission", "permissions.delete"));
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
