public class Operation
{
    public string? Operator { get; set; }
    public double? Value1 { get; set; }
    public double? Value2 { get; set; }

    public double? Result { get; set; }

    private Exception? _error;
    public Exception? Error
    {
        get => _error;
        set
        {
            if (_error == null)
            {
                _error = value;
            }
        }
    }

    public Operation() : this(new OperationDto())
    {
    }

    // Map dto to operation model and validate
    public Operation(OperationDto dto)
    {
        Operator = dto.Operator?.ToString();

        // Try parsing operands
        bool value1Parsed = double.TryParse(dto.Value1?.ToString(), out var v1);
        bool value2Parsed = double.TryParse(dto.Value2?.ToString(), out var v2);

        Value1 = value1Parsed ? v1 : null;
        Value2 = value2Parsed ? v2 : null;

        // Reject numeric operator
        if (double.TryParse(Operator, out _)) 
        {
            Error = new InvalidOperationException("Operator cannot be numeric");
            return;
        }

        // Check for empty operator
        if (string.IsNullOrWhiteSpace(Operator))
        {
            Error = new InvalidOperationException("Operator is missing");
            return;
        }

        // Check for empty both operands
        if (Value1 == null && Value2 == null)
        {
            Error = new InvalidOperationException("Both operands missing or invalid");
            return;
        }
    }

    public OperationType? OperatorType
    {
        get
        {
            // Reject numeric operator
            if (double.TryParse(Operator, out _)) 
            {
                Error = new InvalidOperationException("Operator cannot be numeric");
                return null;
            }

            // Check if supported operation
            if (!Enum.TryParse<OperationType>(Operator, true, out var opEnum))
            {
                Error = new InvalidOperationException("Operator not supported");
                return null;
            }

            return opEnum;
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