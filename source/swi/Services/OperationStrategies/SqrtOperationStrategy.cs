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
            throw new ArgumentException();
        }
        if (a.Value < 0)
        {
            throw new ArgumentException();
        }

        // Execute
        double result = Math.Sqrt(a.Value);
        _logger.LogDebug(SupportedOperator.ToString().ToLower() + "Operation: {A} = {Result}", 
            a.Value, result);
        return result;
    }
}