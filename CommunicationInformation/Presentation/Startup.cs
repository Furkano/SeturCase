using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Services;
using Domanin.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repository;
using MassTransit;
using MediatR;
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
using Setur.Events;

namespace Presentation
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
            services.AddDbContext<PostgreSqlContext>(options =>
            options.UseNpgsql(Configuration["PostgreSqlConnectionString"],
                b => b.MigrationsAssembly("Infrastructure")));

            services.AddScoped(typeof(ICommunicationInfoPostgresRepository<>), typeof(CommunicationInfoPostgresRepository<>));

            services.AddMediatR(typeof(AddCommunicationInfoService).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(RemoveCommunicationInfoService).GetTypeInfo().Assembly);
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CommunicationInfo Presentation", Version = "v1" });
            });
            
            services.AddMediatR(typeof(AddCommunicationInfoService).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(RemoveCommunicationInfoService).GetTypeInfo().Assembly);

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, config2) =>
                {
                    config2.Host(Configuration["RabbitMQConf:Uri"]);
                    config2.UseHealthCheck(context);
                });
            });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Presentation v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            InitializeDatabase(app);
        }
        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PostgreSqlContext>().Database.Migrate();
            }
        }
    }
}
