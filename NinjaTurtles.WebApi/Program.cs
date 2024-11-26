using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
builder.Services.AddControllers(); // Controller'lar için gerekli
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// burada WithOrigins ile domainimiz neyse react projesinde vs onu yazýyuoruz ki burasý dýþýnda baþka bir yerden istek gelirse cors hatasý versin  api endpoint https://localhost:44368/
builder.Services.AddCors(options => options.AddPolicy("AllowOrigin", builder => builder.WithOrigins("http://localhost:3000")));

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
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions?.SecurityKey)

    };
});

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers(); // Controller rotalarýný ekle

app.Run();
