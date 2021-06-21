using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3
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
            services.AddControllersWithViews();
            services.Configure<DeltaRobotDbSettings>(
                Configuration.GetSection(nameof(DeltaRobotDbSettings)));

            services.AddSingleton<IDeltaRobotDbSettings>(sp =>
                sp.GetRequiredService<IOptions<DeltaRobotDbSettings>>().Value);

            services.AddSingleton<RobotPickModelService>();
            
            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());
            
            
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
                app.UseHsts();
                // app.UseExceptionHandler("/Home/Error");
            }
            // app.UseStaticFiles();
            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapControllerRoute(
                //     name: "default",
                //     pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            Console.WriteLine("\n-----------------: Hello Startup.cs");
        }
    }
}
