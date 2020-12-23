using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labmin.Api.Data;
using Labmin.Api.Repositories;
using Labmin.Api.Repositories.EfCore;
using Labmin.Api.Services;
using Labmin.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Labmin.Api
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
            //services.AddScoped(typeof(DbContext), typeof(LabminDbContext));
            
            services.AddDbContext<LabminDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LabminDBContext")));
            services.AddScoped<DbContext, LabminDbContext>();
            services.AddScoped<IRepository<Pool>, EfCoreSqlPoolRepository>();
            services.AddScoped<IPoolService, PoolService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
