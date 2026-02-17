using Microsoft.Extensions.Logging;

public class SqrtOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Sqrt;

    protected override int MinOperands => 1;
    protected override int? MaxOperands => 1;

    public SqrtOperationStrategy(ILogger<SqrtOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        ValidateOperands(operation);

        var value = operation.Operands[0];

        if (value < 0)
        {
            _logger.LogWarning("Invalid {Operation} operation: negative operand. Value: {value}",
                SupportedOperator.ToString().ToLower(), value);
            throw new ArgumentException("Cannot calculate square root of a negative number");
        }

        return Math.Sqrt(value);
    }
}