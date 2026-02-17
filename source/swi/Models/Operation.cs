using System.Text.Json;

public class Operation
{
    public string? Operator { get; set; }
    public List<double> Operands { get; } = new List<double>();

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
        Operator = ParseOperator(dto.Operator);

        var operand1 = ParseOperand(dto.Value1, 1);
        if (Error != null) return;
        Operands.Add(operand1);

        var operand2 = ParseOperand(dto.Value2, 2);
        if (Error != null) return;
        Operands.Add(operand2);
    }

    private string? ParseOperator(JsonElement? element)
    {
        // Check if exists
        if (element == null || element.Value.ValueKind == JsonValueKind.Null)
            return null;

        switch (element.Value.ValueKind)
        {
            case JsonValueKind.String:
                return element.Value.GetString();
            case JsonValueKind.Number:
                // Reject numeric operator
                Error = new InvalidOperationException("Operator cannot be numeric");
                return null;
            default:
                // Unsupported operator type
                Error = new InvalidOperationException("Unsupported operator");
                return null;
        }
    }

    private double ParseOperand(JsonElement? element, int key)
    {
        // Check if exists
        if (element == null || element.Value.ValueKind == JsonValueKind.Null)
        {
            Error = new InvalidOperationException($"Operand {key} is missing");
            return double.NaN;
        }

        switch (element.Value.ValueKind)
        {
            case JsonValueKind.Number:
                return element.Value.GetDouble();
            case JsonValueKind.String:
                // Parse to double
                var str = element.Value.GetString();
                if (!double.TryParse(
                    str, 
                    System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, 
                    System.Globalization.CultureInfo.InvariantCulture, 
                    out var value))
                {
                    Error = new InvalidOperationException($"Operand {key} is invalid");
                    return double.NaN;
                }
                return value;
            default:
                Error = new InvalidOperationException($"Operand {key} is invalid");
                return double.NaN;
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
        var operandString = string.Join(", ",
            Operands.Select(o => o.ToString() ?? "Unknown"));

        return $"{op}: {operandString}";
    }
}