
using Cooperchip.FeedRSSAnalytics.CoreShare.Configurations;
using Cooperchip.FeedRSSAnalytics.Domain.Reposiory.AbtractRepository;
using Cooperchip.FeedRSSAnalytics.Domain.Services.Persistences;
using Cooperchip.FeedRSSAnalytics.Infra.Data.Orm;
using Cooperchip.FeedRSSAnalytics.Infra.Repository.ImplementationsRepository;
using Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.Automappers;
using Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.Extensios;
using Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.FiltersAndAttibutes;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Abstrations.Factory;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations;
using Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

            builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<AppSettings>>().Value);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                            //.WithExposedHeaders("X-Pagination", "X-Pagination-Result"));
            });

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
            builder.Services.AddScoped<IArticleMatrixFactory, ArticleMatrixFactory>();
            builder.Services.AddScoped<IFeedProcessor, FeedProcessor>();
            builder.Services.AddScoped<ITransactionHandler, TransactionHandler>();

            var app = builder.Build();

            app.UseCors("Total");

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