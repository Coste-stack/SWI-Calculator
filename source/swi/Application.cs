using Microsoft.Extensions.Logging;

public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly JsonHelper _jsonHelper;

    public Application(ILogger<Application> logger, JsonHelper jsonHelper)
    {
        _logger = logger;
        _jsonHelper = jsonHelper;
    }

    public async Task RunAsync()
    {
        // Get paths
        var baseDir = AppContext.BaseDirectory;
        var inputPath = Path.Combine(baseDir, "input.json");
        var outputPath = Path.Combine(baseDir, "output.txt");

        // Execute processes
        try
        {
            var operations = await _jsonHelper.ReadOperationsAsync(inputPath);
            foreach (var op in operations)
            {
                _logger.LogInformation("Read operation: {Op}", op);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing operations");
        }
    }
}