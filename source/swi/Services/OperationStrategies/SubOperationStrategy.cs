using Microsoft.Extensions.Logging;

public class SubOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Sub;

    protected override int MinOperands => 2;
    protected override int? MaxOperands => null;

    public SubOperationStrategy(ILogger<SubOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        ValidateOperands(operation);

        return operation.Operands.Aggregate((a, b) => a - b);
    }
}