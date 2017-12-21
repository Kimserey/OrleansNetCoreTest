using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using OrleansNetCoreTest.Grains;
using System;
using System.Threading.Tasks;
using Orleans;

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
            config.Globals.ClusterId = "orleans-test";
            config.AddAzureBlobStorageProvider(providerName: "Default", connectionString: "UseDevelopmentStorage=true", containerName: "bank-accounts");
            config.AddAzureBlobStorageProvider(providerName: "PubSubStore", connectionString: "UseDevelopmentStorage=true", containerName: "dev-pubsub-store");
            config.AddAzureQueueStreamProviderV2("transactions", connectionString: "UseDevelopmentStorage=true");

            var builder = new SiloHostBuilder()
             .UseConfiguration(config)
             .ConfigureApplicationParts(x => x.AddApplicationPart(typeof(BankAccount).Assembly).WithReferences())
             .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
