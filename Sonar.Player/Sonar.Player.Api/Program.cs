using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sonar.Player.Api.Bootstraps;
using Sonar.Player.Api.Filters;
using Sonar.Player.Application.Services;
using Sonar.Player.Application.Services.TracksStorage;
using Sonar.Player.Application.Tools;
using Sonar.Player.Data;
using Sonar.Player.Fakes.ApiClients;
using Sonar.Player.Fakes.Services;
using Sonar.UserProfile.ApiClient;
using Sonar.UserProfile.ApiClient.Interfaces;
using Sonar.UserTracksManagement.ApiClient;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());
builder.Services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithCustomAuthorization();

builder.Services.AddDbContext<PlayerDbContext>(opt => opt.UseSqlite("Filename=player.db"));

builder.Services.AddMediatR(typeof(Sonar.Player.Application.IAssemblyMarker));
builder.Services.AddScoped<ITrackStorage, LocalTrackStorage>();
builder.Services.Decorate<ITrackStorage, TrackConverter>();
builder.Services.Decorate<ITrackStorage, HlsTrackProcessor>();
builder.Services.AddSingleton<ITrackPathBuilder, TrackPathBuilder>();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<IUserTracksApiClient, FakeUserTracksClient>();
builder.Services.AddScoped<IUserTracksApiClient, UserTracksApiClient>(f => new UserTracksApiClient("https://localhost:7055", f.GetRequiredService<HttpClient>()));
builder.Services.AddScoped<IPlaylistApiClient, PlaylistApiClient>(f => new PlaylistApiClient("https://localhost:7055", f.GetRequiredService<HttpClient>()));
builder.Services.AddScoped<IUserApiClient, UserApiClient>(f => new UserApiClient("https://localhost:7062", f.GetRequiredService<HttpClient>()));

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