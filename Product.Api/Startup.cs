using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Products.Api.Services;
using Products.Api.Settings;
using Products.Interface;
using Products.Model;
using Products.Repository;
using System;
using System.Net.Http;
using System.Text.Json;

namespace Products.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(Configuration["CorsUrl"])
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            var settings = Configuration.GetSection("MovieSettings");

            // Configurations
            services.Configure<MovieSettings>(settings);

            services.AddMemoryCache();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.MigrationsAssembly("Products.Api")));

            services.AddSingleton(provider => settings.Get<MovieSettings>());
            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IValidator<Product>, ProductValidator>();
            services.AddTransient<IValidator<ProductOption>, ProductOptionValidator>();
            services.AddHttpClient<IMovieService, MovieService>(client =>
            {
                // Inject header token
                client.DefaultRequestHeaders.Add("x-access-token", settings["AccessToken"]);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5)) // Set lifetime to five minutes
            .AddPolicyHandler(GetRetryPolicy()); // Set retry policy

            services.AddRouting(options => options.LowercaseUrls = true);
            services
                .AddControllers()
                .AddFluentValidation()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger - Products API";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
