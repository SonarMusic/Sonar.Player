using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sonar.Player.Api.Bootstraps;
using Sonar.Player.Api.Filters;
using Sonar.Player.Application.Services;
using Sonar.Player.Application.Services.TracksStorage;
using Sonar.Player.Application.Tools;
using Sonar.Player.Data;
using Sonar.UserProfile.ApiClient;
using Sonar.UserProfile.ApiClient.Interfaces;
using Sonar.UserTracksManagement.ApiClient;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());
builder.Services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithCustomAuthorization();

builder.Services.AddDbContext<PlayerDbContext>(opt => opt.UseSqlite(
    builder.Configuration.GetRequiredSection("ConnectionStrings")
        .GetRequiredSection("Database").Value
));

builder.Services.AddMediatR(typeof(Sonar.Player.Application.IAssemblyMarker));
builder.Services.AddScoped<ITrackStorage, LocalTrackStorage>();
builder.Services.Decorate<ITrackStorage, TrackConverter>();
builder.Services.Decorate<ITrackStorage, HlsTrackProcessor>();
builder.Services.AddSingleton<ITrackPathBuilder, TrackPathBuilder>();
builder.Services.Configure<TrackPathBuilderConfiguration>(builder.Configuration.GetRequiredSection("TrackPaths"));

builder.Services.AddScoped<HttpClient>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTracksApiClient, UserTracksApiClient>(f => new UserTracksApiClient(
    builder.Configuration.GetRequiredSection("ConnectionStrings")
        .GetRequiredSection("UserTracksManagement").Value, f.GetRequiredService<HttpClient>()));
builder.Services.AddScoped<IPlaylistApiClient, PlaylistApiClient>(f => new PlaylistApiClient(
    builder.Configuration.GetRequiredSection("ConnectionStrings")
        .GetRequiredSection("PlaylistManagement").Value, f.GetRequiredService<HttpClient>()));
builder.Services.AddScoped<IUserApiClient, UserApiClient>(f => new UserApiClient(
    builder.Configuration.GetRequiredSection("ConnectionStrings")
        .GetRequiredSection("UserProfile").Value, f.GetRequiredService<HttpClient>()));

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseSwaggerWithUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseSerilogRequestLogging();

app.Run();