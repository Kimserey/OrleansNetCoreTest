using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Runtime.Configuration;
using OrleansNetCoreTest.Interfaces;
using Orleans.Runtime;
using Swashbuckle.AspNetCore.Swagger;
using System.Net;

namespace OrleansNetCoreTest.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IGrainFactory>(x => StartClientWithRetries().Result);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api-v1", new Info { Title = "API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/api-v1/swagger.json", "API v1");
                });
            }

            app.UseMvc();
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            await Task.Delay(3000);

            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    var config = ClientConfiguration.LocalhostSilo();
                    config.Gateways.Add(new IPEndPoint(IPAddress.Loopback, 40001));

                    client = new ClientBuilder()
                        .UseConfiguration(config)
                        .AddApplicationPartsFromReferences(typeof(IBankAccount).Assembly)
                        .ConfigureLogging(logging => logging.AddConsole())
                        .Build();

                    await client.Connect();
                    Console.WriteLine("===========>>> Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }
    }
}
