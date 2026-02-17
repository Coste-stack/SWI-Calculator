using Microsoft.Extensions.Logging;

public abstract class OperationStrategyBase : IOperationStrategy
{
    protected readonly ILogger _logger;

    protected OperationStrategyBase(ILogger logger)
    {
        _logger = logger;
    }

    public abstract OperationType SupportedOperator { get; }

    public abstract int MinOperands { get; }
    public virtual int? MaxOperands => null;

    protected void ValidateOperands(Operation operation)
    {
        if (operation.Operands.Count < MinOperands)
            throw new ArgumentException(
                $"{SupportedOperator} requires at least {MinOperands} operand(s)");

        if (MaxOperands.HasValue && operation.Operands.Count > MaxOperands.Value)
            throw new ArgumentException(
                $"{SupportedOperator} allows at most {MaxOperands.Value} operand(s)");
    }

    public abstract double Execute(Operation operation);
}
