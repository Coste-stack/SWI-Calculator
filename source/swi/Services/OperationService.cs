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
    public Dictionary<string, Operation> ExecuteOperations(Dictionary<string, Operation> operations)
    {
        _logger.LogInformation("Starting execution of {Count} operations", operations.Count);

        foreach (var kvp in operations)
        {
            var key = kvp.Key;
            var operation = kvp.Value;

            _logger.LogDebug("Executing operation: {OperationKey}, {Operation}", key, operation);

            // Skip if operation already has an error
            if (operation.Error != null) 
            {
                _logger.LogWarning("Skipping operation due to error: {Error}", operation.Error);
                continue;
            }

            // Match operation strategy
            var opType = operation.OperatorType;
            if (!opType.HasValue || !_strategyMap.TryGetValue(opType.Value, out var strategy))
            {
                var ex = new InvalidOperationException($"No strategy found for operator '{operation.Operator}'");
                _logger.LogWarning(ex, "No strategy for operation '{OperationKey}'", key);
                operation.Error = ex;
                continue;
            }

            // Calculate result
            try
            {
                var result = strategy.Execute(operation);
                operation.Result = result;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error while executing operation: {OperationKey}", key);
                operation.Error = ex;
            }
        }

        _logger.LogInformation("Finished executing operations");
        return operations;
    }
}