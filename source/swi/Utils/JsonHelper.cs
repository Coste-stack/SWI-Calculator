using swi.Models.Operations;
using swi.Services.Operations;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace swi.Utils;

public class JsonHelper
{
    private readonly ILogger<JsonHelper> _logger;
    private readonly IOperationFactory _operationFactory;

    public JsonHelper(ILogger<JsonHelper> logger, IOperationFactory operationFactory)
    {
        _logger = logger;
        _operationFactory = operationFactory;
    }

    public async Task<Operations> ReadOperationsAsync(string path)
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

        var result = new Operations();

        foreach (var kvp in rawOperations)
        {
            // Map raw strings to model types
            try
            {
                _logger.LogDebug($"Attempting to create operation {kvp.Key} with {kvp.Value}");
                result.Valid[kvp.Key] = _operationFactory.Create(kvp.Value);
                _logger.LogDebug($"Successfully created operation {kvp.Key}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to create operation {kvp.Key}");
                result.Failed[kvp.Key] = ex.Message;
            }
        }

        _logger.LogInformation(
            "Read {Valid} valid operations and {Errors} failed operations",
            result.Valid.Count,
            result.Failed.Count);

        return result;
    }
}