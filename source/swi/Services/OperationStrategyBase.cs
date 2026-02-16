using Microsoft.Extensions.Logging;

public abstract class OperationStrategyBase : IOperationStrategy
{
    protected readonly ILogger _logger;

    protected OperationStrategyBase(ILogger logger)
    {
        _logger = logger;
    }

    public abstract OperationType SupportedOperator { get; }

    public abstract double Execute(Operation operation);
}
