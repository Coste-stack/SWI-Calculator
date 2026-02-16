using Microsoft.Extensions.Logging;

public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly JsonHelper _jsonHelper;
    private readonly TxtHelper _txtHelper;

    public Application(
        ILogger<Application> logger, 
        JsonHelper jsonHelper, 
        TxtHelper txtHelper)
    {
        _logger = logger;
        _jsonHelper = jsonHelper;
        _txtHelper = txtHelper;
    }

    public async Task RunAsync()
    {
        // Get paths
        var baseDir = AppContext.BaseDirectory;
        var inputPath = Path.Combine(baseDir, "input.json");
        var outputPath = Path.Combine(baseDir, "output.txt");

        try
        {
            // Read from json
            var operations = await _jsonHelper.ReadOperationsAsync(inputPath);
            // Debug
            foreach (var op in operations)
            {
                _logger.LogInformation("Read operation: {Op}", op);
            }

            // Mock results
            var results = operations.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value1!.Value);
            
            // Write results to txt
            await _txtHelper.WriteResultsAsync(outputPath, results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing operations");
        }
    }
}