using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Sonar.Player.Api.Bootstraps;

public static class SwaggerConfigurator
{
    public static IServiceCollection AddSwaggerWithCustomAuthorization(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.CustomSchemaIds(type => type.FullName);
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo { Title = "Sonar.Player", Version = "v1" });

                options.AddSecurityDefinition(
                    "Token",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Token to access resources",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                    });

                string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            }
        );

        return services;
    }

    public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
