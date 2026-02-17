using Microsoft.Extensions.Logging;

public class AddOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Add;

    protected override int MinOperands => 2;
    protected override int? MaxOperands => null;

    public AddOperationStrategy(ILogger<AddOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        ValidateOperands(operation);

        return operation.Operands.Sum();
    }
}