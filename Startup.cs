using GetECINo.Middleware;
using GetECINo.Models;
using GetECINo.Repository;
using GETECINo.Helpers;
using GETECINo.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StackExchange.Profiling;

namespace GETECINo
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
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
            services.AddRazorPages();
            services.Configure<DatabaseSettings>(options => {
                Configuration.GetSection("DatabaseSettings").Bind(options);
            });
            services.AddSingleton<IDataRepository, DataRepository>();

            services.Configure<UserSettings>(options =>
            { 
                Configuration.GetSection("UserSettings").Bind(options); 
            });
            services.AddSwaggerDocument();
            services.AddControllers();

            if (Configuration["UserSettings:EnableMiniProfiler"] == "True")
            {
                services.AddMiniProfiler(options =>
                {
                    options.RouteBasePath = Configuration["UserSettings:BasePath"] + "/miniprofiler";
                    options.IgnorePath("/Swagger/");
                    options.IgnorePath("/miniprofiler");

                });
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (Configuration["UserSettings:EnableMiniProfiler"] == "True")
            {
                app.UseMiniProfiler();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(AuditMiddleware));
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
