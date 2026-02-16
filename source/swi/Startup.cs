using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class Startup
{
    public static async Task Main()
    {
        // Setup dependency injection
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Debug);   
            })
            .AddSingleton<Application>()
            .AddSingleton<JsonHelper>()
            .AddSingleton<TxtHelper>()
            .BuildServiceProvider();

        // Use logger
        var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
        logger.LogDebug("Starting application");
        
        // Use application
        var app = serviceProvider.GetRequiredService<Application>();
        await app.RunAsync();
    }
}