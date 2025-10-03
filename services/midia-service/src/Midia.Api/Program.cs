using Amazon.S3;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Midia.Application.Interfaces;
using Midia.Application.Mappings;
using Midia.Domain.Factories;
using Midia.Domain.Factories.Interfaces;
using Midia.Domain.Repositories;
using Midia.Infrastructure.Data;
using Midia.Infrastructure.Repositories;
using Midia.Infrastructure.Storage;
using Midia.Infrastructure.Storage.Implementations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MediaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<IMediaFactory, MediaFactory>();


var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();


builder.Services.AddScoped<LocalStorageStrategy>();
builder.Services.AddScoped<AwsS3StorageStrategy>();
builder.Services.AddScoped<StorageStrategyFactory>();



builder.Services.AddScoped<IMediaService>(sp =>
{
    var factory = sp.GetRequiredService<StorageStrategyFactory>();
    var storage = factory.Create();

    return new MediaService(
        sp.GetRequiredService<IUnitOfWork>(),
        sp.GetRequiredService<IMediaFactory>(),
        storage,
        sp.GetRequiredService<IMapper>()
    );
});



builder.Services.AddAutoMapper(cfg =>
{
    
}, typeof(MediaProfile).Assembly);



builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        
        options.SuppressModelStateInvalidFilter = true;
    });


builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])
        )
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
