public interface IOperationStrategy
{
    OperationType SupportedOperator { get; }
    double Execute(Operation operation);
}