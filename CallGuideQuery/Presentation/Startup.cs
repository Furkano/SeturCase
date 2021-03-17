using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Consumers;
using Domain.Interfaces;
using Infrastructure.Repository;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            

            services.AddScoped(typeof(ICallGuideMongoDbRepository<>),typeof(CallGuideMongoDbRepository<>));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CallGuideQuery Presentation", Version = "v1" });
            });

            services.AddMassTransit(config=>{
                config.UsingRabbitMq((context,config2) =>
                {
                    config2.Host(new Uri(Configuration["RabbitMQConf:Uri"]),
                    auth=>{
                        auth.Username(Configuration["RabbitMQConf:Username"]);
                        auth.Password(Configuration["RabbitMQConf:Password"]);
                    });
                    config2.ReceiveEndpoint("add-person-call-guide-queue",
                    consum =>{
                        consum.ConfigureConsumer<AddPersonCallGuideConsumer>(context);
                    });
                    config2.ReceiveEndpoint("remove-person-call-guide-queue",
                    consum =>{
                        consum.ConfigureConsumer<RemovePersonCallGuideConsumer>(context);
                    });
                    config2.ReceiveEndpoint("add-communication-info-queue",
                    consum =>{
                        consum.ConfigureConsumer<AddCommunicationInfoConsumer>(context);
                    });
                    config2.ReceiveEndpoint("remove-communication-info-queue",
                    consum =>{
                        consum.ConfigureConsumer<RemoveCommunicationInfoConsumer>(context);
                    });
                });
            });
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
