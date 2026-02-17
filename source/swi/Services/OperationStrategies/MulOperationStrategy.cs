using Microsoft.Extensions.Logging;

public class MulOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Mul;

    public override int MinOperands => 2;
    public override int? MaxOperands => null;
    
    public MulOperationStrategy(ILogger<MulOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        ValidateOperands(operation);
        
        return operation.Operands.Aggregate((a, b) => a * b);
    }
}