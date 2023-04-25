
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;
using Cooperchip.FeedRSSAnalytics.Infra.Repository.ImplementationsRepository;
using Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.Automappers;
using Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.Extensios;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddControllers();
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