using System.Text.Json.Serialization;

public class Operation
{
    public string? Operator { get; set; }
    public double? Value1 { get; set; }
    public double? Value2 { get; set; }

    public Operation() : this(new OperationDto())
    {
    }

    public Operation(OperationDto dto)
    {
        // Map dto to operation model
        Operator = dto.Operator?.ToString();
        Value1 = double.TryParse(dto.Value1?.ToString(), out var v1) ? v1 : null;
        Value2 = double.TryParse(dto.Value2?.ToString(), out var v2) ? v2 : null;
    }

    public OperationType OperatorType
    {
        get
        {
            // Reject numeric strings
            if (double.TryParse(Operator, out _)) 
            {
                return OperationType.Unknown;
            }

            // Check if supported operation
            if (!string.IsNullOrWhiteSpace(Operator) &&
                Enum.TryParse<OperationType>(Operator, true, out var opEnum))
            {
                return opEnum;
            }
            
            return OperationType.Unknown;
        }
    }

    public override string ToString()
    {
        var op = Operator ?? "Unknown";
        var val1 = Value1.HasValue 
            ? Value1.Value.ToString() 
            : "Unknown";

        return Value2.HasValue
            ? $"{op}: {val1}, {Value2.Value}"
            : $"{op}: {val1}";
    }
}