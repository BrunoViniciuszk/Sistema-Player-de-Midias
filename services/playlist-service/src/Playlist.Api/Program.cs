using Microsoft.EntityFrameworkCore;
using Playlist.Application.Interfaces;
using Playlist.Application.Mappings;
using Playlist.Application.Services;
using Playlist.Domain.Repositories;
using Playlist.Infrastructure.Data;
using Playlist.Infrastructure.Repositories;
using Playlist.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PlaylistDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddAutoMapper(cfg => cfg.AddProfile<PlaylistProfile>());


builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
