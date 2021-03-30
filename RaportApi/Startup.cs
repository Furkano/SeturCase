using System.Reflection;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RaportApi.Consumers;
using RaportApi.Context;
using RaportApi.Services;

namespace RaportApi
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
            options.UseNpgsql(Configuration["PostgreSqlConnectionString"]));
            
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RaportApi", Version = "v1" });
            });
            
            services.AddMediatR(typeof(CreateRaportService).GetTypeInfo().Assembly);

            services.AddMassTransit(config=>{
                config.AddConsumer<UpdateRaportConsumer>();
                config.UsingRabbitMq((context,config2) =>
                {
                    config2.Host(Configuration["RabbitMQConf:Uri"]);
                    config2.UseHealthCheck(context);
                    config2.ReceiveEndpoint("update-raport-queue",
                    consum =>{
                        consum.ConfigureConsumer<UpdateRaportConsumer>(context);
                    });
                   
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RaportApi v1"));
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
