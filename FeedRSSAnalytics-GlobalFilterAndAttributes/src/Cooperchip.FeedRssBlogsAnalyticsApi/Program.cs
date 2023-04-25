
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;
using Cooperchip.FeedRSSAnalytics.Infra.Repository.ImplementationsRepository;
using Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.Automappers;
using Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.Extensios;
using Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.FiltersAndAttibutes;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Cooperchip.FeedRssBlogsAnalyticsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;

            builder.Services
                .AddDbContext<ApplicationDbContext>(opt =>
                        opt.UseSqlServer(configuration.GetConnectionString("ConnStr")));

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<GlobalProducesResponseTypeFilter>();
                opt.Filters.Add(new GlobalProducesResponseTypeAttibutes(typeof(ErrorResponse), 400));
                opt.Filters.Add(new GlobalProducesResponseTypeAttibutes(typeof(ErrorResponse), 404));
                opt.Filters.Add(new GlobalProducesResponseTypeAttibutes(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized));
                opt.Filters.Add(new GlobalProducesResponseTypeAttibutes(typeof(ErrorResponse), (int)HttpStatusCode.MethodNotAllowed));
            });

            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerConfiguration();


            builder.Services.AddScoped<IQueryRepository, QueryRepository>();
            builder.Services.AddScoped<IQueryADORepository, QueryADORepository>();

            builder.Services.AddScoped<IArticleMatrixRepository, ArticleMatrixRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwaggerConfiguration();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}