using System.Text.Json;
using Microsoft.Extensions.Logging;

public class JsonHelper
{
    private readonly ILogger<JsonHelper> _logger;

    public JsonHelper(ILogger<JsonHelper> logger)
    {
        _logger = logger;
    }
    
    public async Task<Dictionary<string, Operation>> ReadOperationsAsync(string path)
    {
        if (!File.Exists(path))
        {
            _logger.LogError("File not found: {Path}", path);
            throw new FileNotFoundException($"Input file not found: {path}");
        }
        
        // Read operations
        using FileStream jsonStream = File.OpenRead(path);
        var operations = await JsonSerializer.DeserializeAsync<Dictionary<string, Operation>>(jsonStream);
        
        if (operations == null) 
        {
            _logger.LogError("Failed to deserialize JSON from {Path}", path);
            throw new InvalidDataException($"Failed to deserialize JSON from {path}");
        }

        _logger.LogInformation("Successfully read {Count} operations", operations.Count);
        return operations;
    }
}