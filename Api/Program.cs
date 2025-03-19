using Api.Common;
using Api.Middlewares;
using Core.Factories;
using Core.Services;
using System.Text.Json.Serialization;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IGitIssueServiceFactory, GitIssueServiceFactory>();
            builder.Services.AddScoped<IGitHubIssueService, GitHubIssueService>();
            builder.Services.AddScoped<IGitLabIssueService, GitLabIssueService>();

            builder.Services.AddScoped<ExceptionLoggerMiddleware>();
            builder.Services.AddScoped<IProblemDetailsFactory, ProblemDetailsFactory>();


            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                var converter = new JsonStringEnumConverter();
                options.JsonSerializerOptions.Converters.Add(converter);
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.EnableAnnotations();
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Git Service Api",
                    Version = "v1",
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionLoggerMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
