using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using NinjaTurtles.Business.DependencyResolvers.Autofac;
using NinjaTurtles.Core.DependencyResolver;
using NinjaTurtles.Core.Extensions;
using NinjaTurtles.Core.NetCoreConfiguration;
using NinjaTurtles.Core.Utilities.IoC;
using NinjaTurtles.Core.Utilities.Security.Enctyption;
using NinjaTurtles.Core.Utilities.Security.Jwt;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration from appsettings
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration);
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddDependencyResolvers(new ICoreModule[]
{
    new CoreModule(),
});
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacBusinessModule());
});

// Add services to the container
builder.Services.AddControllers(); // Controller'lar için gerekli
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "NinjaTurtles API",
        Description = "API Documentation"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: 'Bearer12345abcdef'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// JWT
var tokenOptions = AppConfig.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false, // token olsun yeter diyorsak bu false olacak, true olacaksa süreye bakar
        ValidIssuer = tokenOptions?.Issuer,
        ValidAudience = tokenOptions?.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions?.SecurityKey),
        ClockSkew = TimeSpan.Zero
    };

    // Events configuration - log with Serilog
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            Log.Debug("JWT received for {Path}", context.HttpContext.Request.Path);
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Log.Warning(context.Exception, "JWT authentication failed for {Path}", context.HttpContext.Request.Path);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Log.Information("JWT validated for {User}", context.Principal?.Identity?.Name ?? "anonymous");
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorization();

var app = builder.Build();

// Global exception handling (custom middleware)
app.UseMiddleware<NinjaTurtles.WebApi.Middlewares.ExceptionHandlingMiddleware>();

// Base path (if any)
app.UsePathBase("/qrapi");
app.UseHttpsRedirection();

app.UseRouting();

// Do NOT log raw tokens in production; only presence
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("Authorization"))
    {
        Log.Debug("Authorization header present for {Path}", context.Request.Path);
    }
    else
    {
        Log.Debug("Authorization header missing for {Path}", context.Request.Path);
    }
    await next.Invoke();
});

app.UseCors(_ => _
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
.WithExposedHeaders("Content-Disposition"));

// Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

// Serilog request logging (HTTP pipeline)
app.UseSerilogRequestLogging(opts =>
{
    opts.EnrichDiagnosticContext = (diag, http) =>
    {
        diag.Set("ClientIP", http.Connection.RemoteIpAddress?.ToString());
        diag.Set("UserAgent", http.Request.Headers["User-Agent"].ToString());
        diag.Set("UserName", http.User?.Identity?.Name);
    };
});

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
