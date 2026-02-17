using Microsoft.Extensions.Logging;

public class SubOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Sub;

    public override int MinOperands => 2;
    public override int? MaxOperands => null;

    public SubOperationStrategy(ILogger<SubOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        ValidateOperands(operation);

        return operation.Operands.Aggregate((a, b) => a - b);
    }
}