using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlashParcsLite.API.Hubs;
using FlashParcsLite.Data;
using FlashParcsLite.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlashParcsLite.API
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

            var dbLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var db = Path.Join(dbLocation, "parking.db");
            services.AddDbContext<ParkingContext>(options => options.UseSqlite($"Data Source = {db}"));

            services.AddScoped<IParkingLocationRepository, ParkingLocationRepository>();


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                //.AllowAnyOrigin()
                .WithOrigins("https://localhost:45605")
                .AllowCredentials()                
                //.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                );
            });
            services.AddSignalR();
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ParkingHub>("/parkingHub");
            });
        }
    }
}
