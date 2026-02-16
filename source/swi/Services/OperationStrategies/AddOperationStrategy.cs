using Microsoft.Extensions.Logging;

public class AddOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Add;

    public AddOperationStrategy(ILogger<AddOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        // Validate
        var a = operation.Value1;
        var b = operation.Value2;
        if (!a.HasValue || !b.HasValue)
        {
            throw new ArgumentException();
        }

        // Execute
        double result = a.Value + b.Value;
        _logger.LogDebug(SupportedOperator.ToString().ToLower() + "Operation: {A}, {B} = {Result}", 
            a.Value, b.Value, result);
        return result;
    }
}