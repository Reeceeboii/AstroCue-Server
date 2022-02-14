namespace AstroCue.Server
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// ASP .NET Core main program class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">List of string arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates a new <see cref="IHostBuilder"/> instance
        /// </summary>
        /// <param name="args">List of string arguments</param>
        /// <returns>An instance of <see cref="IHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
