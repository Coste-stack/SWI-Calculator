using swi.Models.Operations;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace swi.Services.Operations;

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
    public Dictionary<string, string> ExecuteOperations(Dictionary<string, Operation> operations)
    {
        _logger.LogInformation("Starting execution of {Count} operations", operations.Count);

        var results = new Dictionary<string, string>();

        foreach (var (key, operation) in operations)
        {
            _logger.LogDebug("Executing operation: {OperationKey}, {Operation}", key, operation);

            // Match operation strategy
            var opType = operation.OperatorType;
            if (!_strategyMap.TryGetValue(opType, out var strategy))
            {
                var ex = new InvalidOperationException($"No strategy found for operator '{operation.OperatorType}'");
                _logger.LogWarning(ex, "No strategy for operation '{OperationKey}'", key);
                results[key] = ex.Message;
                continue;
            }

            // Calculate result
            try
            {
                var result = strategy.Execute(operation);
                results[key] = result.ToString(CultureInfo.InvariantCulture);;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error while executing operation: {OperationKey}", key);
                results[key] = ex.Message;
            }
        }

        _logger.LogInformation("Finished executing operations");
        return results;
    }
}