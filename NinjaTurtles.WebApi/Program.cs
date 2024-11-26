using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using NinjaTurtles.Business.DependencyResolvers.Autofac;
using NinjaTurtles.Core.NetCoreConfiguration;
using NinjaTurtles.Core.Utilities.Security.Enctyption;
using NinjaTurtles.Core.Utilities.Security.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacBusinessModule());
});

// Add services to the container
builder.Services.AddControllers(); // Controller'lar i�in gerekli
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// burada WithOrigins ile domainimiz neyse react projesinde vs onu yaz�yuoruz ki buras� d���nda ba�ka bir yerden istek gelirse cors hatas� versin  api endpoint https://localhost:44368/
//builder.Services.AddCors(options => options.AddPolicy("AllowOrigin", builder => builder.WithOrigins("http://localhost:44368")));

var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("JwtEventsLogger");

var tokenOptions = AppConfig.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false, // token olsun yeter diyorsak bu false olacak, true olacaksa s�reye bakar
        ValidIssuer = tokenOptions?.Issuer,
        ValidAudience = tokenOptions?.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions?.SecurityKey)
    };

    // Events yap�land�rmas�
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            logger.LogError($"Authentication failed: {context?.Exception?.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            logger.LogInformation($"Token validated for: {context?.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        }
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline

app.UseSwagger();
app.UseSwaggerUI();

//app.UseCors(builder => builder.WithOrigins("http://localhost:44368").AllowAnyHeader());

app.UseHttpsRedirection();

app.UseRouting();

// UseAuthentication �stte olmal�  UseAuthorization a�a��da olmal�, UseAuthentication eve girmek i�in izindir, UseAuthorization evin i�indeki mutfa�a girmek i�in role gibi d���nebiliriz
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers(); // Controller rotalar�n� ekle

app.Run();
