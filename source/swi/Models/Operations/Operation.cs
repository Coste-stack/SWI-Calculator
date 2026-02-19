namespace swi.Models.Operations;

public class Operation
{
    public OperationType OperatorType { get; set; }
    public IReadOnlyList<double> Operands { get; }

    public Operation(OperationType operatorType, IEnumerable<double> operands)
    {
        OperatorType = operatorType;
        Operands = operands.ToList().AsReadOnly();
    }

    public override string ToString()
    {
        return $"{OperatorType}: {string.Join(", ", Operands)}";
    }
}