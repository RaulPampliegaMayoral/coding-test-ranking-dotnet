using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using coding_test_ranking.infrastructure.persistence.repositories;
using coding_test_ranking.infrastructure.services.Calculator;
using coding_test_ranking.infrastructure.services;
using coding_test_ranking.infrastructure.persistence.models;

namespace coding_test_ranking
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
            services.AddControllers();

            services.AddMvc();

            services.AddSingleton<IRepository<AdVO>, InMemoryPersistence<AdVO>>();
            services.AddSingleton<IRepository<PictureVO>, InMemoryPersistence<PictureVO>>();
            services.AddScoped<IAdsRepository, AdsRepository>();
            services.AddScoped<IPicturesRepository, PicturesRepository>();
            services.AddScoped<IRuleCalculator, RuleCalculator>();
            services.AddScoped<IAdsService, AdsService>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}
