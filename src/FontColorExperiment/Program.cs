using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FontColorExperiment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            // perform main initialization
            FontColorExperiment.Main.Init();

            host.Run();
        }
    }
}
