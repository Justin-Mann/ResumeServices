using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyResumeAPI {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {

                    webBuilder.ConfigureKestrel(serverOptions => {
                        serverOptions.AddServerHeader = false;
                    });

                    webBuilder.ConfigureLogging(builder => {
                        builder.ClearProviders();
                        builder.AddConsole();
                        builder.AddDebug();
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
