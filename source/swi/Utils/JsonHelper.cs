using Microsoft.Extensions.Logging;
using System.Text.Json;

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
        
        // Open stream
        using FileStream jsonStream = File.OpenRead(path);
        // Handle case
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        // Read operations
        var rawOperations = await JsonSerializer.DeserializeAsync<Dictionary<string, OperationDto>>(jsonStream, options)
            ?? new Dictionary<string, OperationDto>();

        var operations = new Dictionary<string, Operation>();

        foreach (var kvp in rawOperations)
        {
            // Map raw strings to model types
            try
            {
                operations[kvp.Key] = new Operation(kvp.Value);
            }
            catch
            {
                _logger.LogWarning("Failed to parse operation {Name}", kvp.Key);
                operations[kvp.Key] = new Operation();
            }
        }

        _logger.LogInformation("Successfully read {Count} operations", operations.Count);
        return operations;
    }
}