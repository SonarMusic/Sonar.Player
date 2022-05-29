using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sonar.Player.Api.Bootstrappers;
using Sonar.Player.Application.Services;
using Sonar.Player.Application.Services.TracksStorage;
using Sonar.Player.Application.Tools;
using Sonar.Player.Data;
using Sonar.Player.Fakes.ApiClients;
using Sonar.Player.Fakes.Services;
using Sonar.UserTracksManagement.ApiClient;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithCustomAuthorization();

builder.Services.AddDbContext<PlayerDbContext>(opt => opt.UseSqlite("Filename=player.db"));

builder.Services.AddMediatR(typeof(Sonar.Player.Application.IAssemblyMarker));

builder.Services.AddScoped<IUserService, FakeUserService>();
builder.Services.AddScoped<IUserTracksApiClient, FakeUserTracksClient>();

builder.Services.AddScoped<ITrackStorage, LocalTrackStorage>();
builder.Services.Decorate<ITrackStorage, HlsTrackProcessor>();
builder.Services.AddSingleton<ITrackPathBuilder, TrackPathBuilder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseSerilogRequestLogging();

app.Run();