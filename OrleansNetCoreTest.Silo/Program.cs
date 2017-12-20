﻿using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using OrleansNetCoreTest.Grains;
using System;
using System.Threading.Tasks;
using Orleans.Storage;

namespace OrleansNetCoreTest.Silo
{
    class Program
    {
        static void Main(string[] args)
        {
            RunMainAsync().Wait();
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("===========>>> BankAccount Host started. Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.AddAzureBlobStorageProvider(providerName: "default", connectionString: "UseDevelopmentStorage=true", containerName: "bank-accounts");
            config.AddSimpleMessageStreamProvider("transactions", true);

            var builder = new SiloHostBuilder()
             .UseConfiguration(config)
             .AddApplicationPartsFromReferences(typeof(BankAccount).Assembly)
             .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
