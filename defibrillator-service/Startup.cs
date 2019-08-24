using DefibrillatorService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: ApiController]
namespace DefibrillatorService
{
    public class StartupOptions
    {
        public string DoctorHelpRestUrl { get; set; }
        public string OSMRestUrl { get; set; }
        public string DefiWebRestUrl { get; set; }
        public string DefiWebAppId { get; set; }
        public string DefiWebPassword { get; set; }
    }

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOptions<StartupOptions>();
            services.Configure<StartupOptions>(Configuration);
            services.AddMemoryCache();
            services.AddSingleton<ILocationService, LocationServiceByDoctorHelp>();
            services.AddSingleton<IDefibrillatorService, DefibrillatorServiceByDefiWeb>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
