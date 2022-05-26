using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Sonar.Player.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.CustomSchemaIds(type => type.FullName);
        options.SwaggerDoc(
            "v1", 
            new OpenApiInfo(){Title = "Sonar.Player", Version = "v1"});
        
        options.AddSecurityDefinition("Token", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Token to access resources",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
);
builder.Services.AddDbContext<PlayerDbContext>(opt => opt.UseSqlite("Filename=player.db"));
builder.Services.AddMediatR(typeof(Sonar.Player.Application.IAssemblyMarker));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSerilogRequestLogging();

app.Run();