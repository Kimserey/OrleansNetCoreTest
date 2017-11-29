using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using OrleansNetCoreTest.UserGrains;
using System;
using System.Threading.Tasks;

namespace OrleansNetCoreTest.UserSilo
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
                Console.WriteLine("===========>>> UserAccount Host started. Press Enter to terminate...");
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
            var config = new ClusterConfiguration();
            config.StandardLoad();

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .AddApplicationPartsFromReferences(typeof(UserAccount).Assembly)
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
