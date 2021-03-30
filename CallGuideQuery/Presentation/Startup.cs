using System;
using System.Reflection;
using Application.Consumers;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Repository;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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
            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton<IMongoSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value);
            

            services.AddScoped(typeof(IMongoRepository),typeof(CallGuideMongoDbRepository));
            services.AddScoped(typeof(IRaportRepository),typeof(RaportRepository));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CallGuideQuery Presentation", Version = "v1" });
            });
            services.AddMediatR(typeof(GetAllCallGuideWithUserIdService).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetDetailAllCallGuidehUserIdService).GetTypeInfo().Assembly);

            services.AddMassTransit(config=>{
                config.AddConsumer<AddPersonCallGuideConsumer>();
                config.AddConsumer<RemovePersonCallGuideConsumer>();
                config.AddConsumer<AddCommunicationInfoConsumer>();
                config.AddConsumer<RemoveCommunicationInfoConsumer>();
                config.AddConsumer<CreatedRaportConsumer>();
                config.UsingRabbitMq((context,config2) =>
                {
                    config2.Host(Configuration["RabbitMQConf:Uri"]);
                    config2.UseHealthCheck(context);
                    config2.ReceiveEndpoint("add-communication-info-queue",
                    consum =>{
                        consum.ConfigureConsumer<AddCommunicationInfoConsumer>(context);
                    });
                    config2.ReceiveEndpoint("remove-communication-info-queue",
                    consum =>{
                        consum.ConfigureConsumer<RemoveCommunicationInfoConsumer>(context);
                    });
                    config2.ReceiveEndpoint("add-person-CallGuide-queue",
                    consum =>{
                        consum.ConfigureConsumer<AddPersonCallGuideConsumer>(context);
                    });
                    config2.ReceiveEndpoint("remove-person-CallGuide-queue",
                    consum =>{
                        consum.ConfigureConsumer<RemovePersonCallGuideConsumer>(context);
                    });
                    config2.ReceiveEndpoint("created-raport-queue",
                        consum => {
                        consum.ConfigureConsumer<CreatedRaportConsumer>(context);
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Presentation v1"));
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
