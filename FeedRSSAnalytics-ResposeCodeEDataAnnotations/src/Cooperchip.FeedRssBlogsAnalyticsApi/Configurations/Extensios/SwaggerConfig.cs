using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.Extensios
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Web Scraping com Asp.Net Core 7 & Angular 15",
                    Description = "Esta API serve recursos para consumo Feed RSS Web Scraping.",
                    Contact = new OpenApiContact() { Name = "Cooperchip, Inc.", Email = "carlos.itdeveloper@gmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
                    TermsOfService = new Uri("https://cooperchip.com.br")
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));



                #region: Security
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                //    Name = "Authorization",
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        new string[] {}
                //    }
                //});
                #endregion
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cooperchip | Web Scraping - v1");
            });

            return app;
        }
    }
}
