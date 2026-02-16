using Microsoft.Extensions.Logging;

public class OperationService
{
    private readonly ILogger<OperationService> _logger;
    private readonly Dictionary<OperationType, IOperationStrategy> _strategyMap;

    public OperationService(
        ILogger<OperationService> logger,
        IEnumerable<IOperationStrategy> strategies)
    {
        _logger = logger;
        _strategyMap = strategies.ToDictionary(s => s.SupportedOperator);
    }
    public Dictionary<string, string> ExecuteOperations(IReadOnlyDictionary<string, Operation> operations)
    {
        _logger.LogInformation("Starting execution of {Count} operations", operations.Count);

        var results = new Dictionary<string, string>();

        foreach (var kvp in operations)
        {
            var key = kvp.Key;
            var operation = kvp.Value;

            _logger.LogDebug("Executing operation: {OperationKey}, {Operation}", key, operation);

            // Check if supported operation
            if (operation.OperatorType == OperationType.Unknown)
            {
                results[key] = "Unknown operation";
            }

            // Match operation strategy
            if (!_strategyMap.TryGetValue(operation.OperatorType, out var strategy))
            {
                _logger.LogWarning("No strategy found for operator '{Operator}'", operation.Operator);
                continue;
            }

            // Calculate result
            try
            {
                var result = strategy.Execute(operation);
                results[key] = result.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error while executing operation: {OperationKey}", key);
                results[key] = "Error";
            }
        }

        _logger.LogInformation("Finished executing operations");
        return results;
    }
}