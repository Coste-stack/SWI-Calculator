using Microsoft.Extensions.Logging;

public class TxtHelper
{
    private readonly ILogger<TxtHelper> _logger;

    public TxtHelper(ILogger<TxtHelper> logger)
    {
        _logger = logger;
    }
    
    public async Task WriteResultsAsync(string path, Dictionary<string, string> operationResults)
    {
        _logger.LogInformation("Writing {Count} results to {Path}", operationResults.Count, path);

        try
        {
            using var writer = new StreamWriter(path, append: false);
            foreach (KeyValuePair<string, string> item in operationResults)
            {
                string line = $"{item.Key}: {item.Value}";
                await writer.WriteLineAsync(line);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write results to {Path}", path);
            throw;
        }

        _logger.LogInformation("Successfully wrote results to {Path}", path);
    }
}