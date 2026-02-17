using Microsoft.Extensions.Logging;

public class SqrtOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Sqrt;

    public SqrtOperationStrategy(ILogger<SqrtOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        // Validate
        var a = operation.Value1;
        if (!a.HasValue)
        {
            _logger.LogWarning("Invalid {Operation} operation: missing operand. Value1: {A}",
                SupportedOperator.ToString().ToLower(), "null");
            throw new ArgumentException("Invalid operand");
        }
        if (a.Value < 0)
        {
            _logger.LogWarning("Invalid {Operation} operation: negative operand. Value1: {A}",
                SupportedOperator.ToString().ToLower(), a.Value);
            throw new ArgumentException("Cannot calculate square root of a negative number");
        }

        // Execute
        double result = Math.Sqrt(a.Value);
        _logger.LogDebug(SupportedOperator.ToString().ToLower() + "Operation: {A} = {Result}", 
            a.Value, result);
        return result;
    }
}