using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScooterRental;
using ScooterRental.Core.Models;
using ScooterRental.Db;
using ScooterRental.Interfaces;
using ScooterRental.Core.Services;
using ScooterRental.Services;

namespace ScooterRentalApi
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScooterRentalApi", Version = "v1" });
            });
            services.AddDbContext<ScooterRentalDbContext>(options =>
            {
                options.UseSqlite("Filename=ScooterRental.db");
            });

            services.AddScoped<IScooterService, ScooterService>();
            services.AddScoped<IEntityService<Rent>, EntityService<Rent>>();
            services.AddScoped<IEntityService<Company>, EntityService<Company>>();
            services.AddScoped<IEntityService<Scooter>, EntityService<Scooter>>();
            services.AddScoped<IRentalCompany, RentalCompany>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScooterRentalApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
