public interface IOperationStrategy
{
    OperationType SupportedOperator { get; }
    abstract int MinOperands { get; }
    virtual int? MaxOperands => null;
    double Execute(Operation operation);
}