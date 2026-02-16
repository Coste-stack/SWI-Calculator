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
                    .SetMinimumLevel(LogLevel.Trace);
            })
            .AddSingleton<Application>()
            .AddSingleton<JsonHelper>()
            .AddSingleton<TxtHelper>()
            .AddSingleton<IOperationStrategy, AddOperationStrategy>()
            .AddSingleton<IOperationStrategy, SubOperationStrategy>()
            .AddSingleton<IOperationStrategy, MulOperationStrategy>()
            .AddSingleton<IOperationStrategy, SqrtOperationStrategy>()
            .AddSingleton<OperationService>()
            .BuildServiceProvider();

        // Use logger
        var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
        logger.LogInformation("Starting application");
        
        // Use application
        var app = serviceProvider.GetRequiredService<Application>();
        await app.RunAsync();
    }
}