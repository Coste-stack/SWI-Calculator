using Microsoft.Extensions.Logging;

public class SubOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Sub;

    public SubOperationStrategy(ILogger<SubOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        // Validate
        var a = operation.Value1;
        var b = operation.Value2;
        if (!a.HasValue || !b.HasValue)
        {
            _logger.LogWarning("Invalid {Operation} operation: missing operand(s). Value1: {A}, Value2: {B}",
                SupportedOperator.ToString().ToLower(), a.HasValue ? a.Value.ToString() : "null", b.HasValue ? b.Value.ToString() : "null");
            throw new ArgumentException("Missing operand(s) for sub operation.");
        }

        // Execute
        double result = a.Value - b.Value;
        _logger.LogDebug(SupportedOperator.ToString().ToLower() + "Operation: {A}, {B} = {Result}", 
            a.Value, b.Value, result);
        return result;
    }
}