using swi.Models.Operations;
using Microsoft.Extensions.Logging;

namespace swi.Services.Operations.OperationStrategies;

public class AddOperationStrategy : OperationStrategyBase
{
    public override OperationType SupportedOperator => OperationType.Add;

    public override int MinOperands => 2;
    public override int? MaxOperands => null;

    public AddOperationStrategy(ILogger<AddOperationStrategy> logger) : base(logger)
    {
    }

    public override double Execute(Operation operation)
    {
        ValidateOperands(operation);

        return operation.Operands.Sum();
    }
}