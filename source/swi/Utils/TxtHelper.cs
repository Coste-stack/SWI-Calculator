using Microsoft.Extensions.Logging;

public class TxtHelper
{
    private readonly ILogger<TxtHelper> _logger;

    public TxtHelper(ILogger<TxtHelper> logger)
    {
        _logger = logger;
    }
    
    public async Task WriteResultsAsync(string path, Dictionary<string, Operation> operations)
    {
        _logger.LogInformation("Writing {Count} results to {Path}", operations.Count, path);

        try
        {
            using var writer = new StreamWriter(path, append: false);
            foreach (KeyValuePair<string, Operation> item in operations)
            {
                string result = item.Value.Result?.ToString() 
                                ?? item.Value.Error?.Message 
                                ?? "Error";
                string line = $"{item.Key}: {result}";
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