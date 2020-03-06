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
using Products.Api.Services;
using Products.Api.Settings;
using Products.Interface;
using Products.Model;
using Products.Repository;

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

            // Configurations
            services.Configure<MovieSettings>(Configuration.GetSection("MovieSettings"));

            services.AddMemoryCache();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("Products.Api")));
            
            services.AddSingleton(provider => Configuration.GetSection("MovieSettings").Get<MovieSettings>());
            services.AddTransient<IMovieService, MovieService>();

            services.AddTransient<IProductsRepository, ProductsRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IValidator<Product>, ProductValidator>();
            services.AddTransient<IValidator<ProductOption>, ProductOptionValidator>();
            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
            .AddFluentValidation();

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
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
