using System.Text.Json;

public class OperationFactory : IOperationFactory
{
    // Map dto to operation model and validate
    public Operation Create(OperationDto dto)
    {
        var operatorType = ParseOperator(dto.Operator);

        var operand1 = ParseOperand(dto.Value1, 1);
        var operand2 = ParseOperand(dto.Value2, 2);

        return new Operation(operatorType, new[] { operand1, operand2 });
    }

    private OperationType ParseOperator(JsonElement? element)
    {
        // Check if exists
        if (element == null || element.Value.ValueKind == JsonValueKind.Null)
            throw new InvalidOperationException("Operator is missing");

        // Reject numeric operator
        if (element.Value.ValueKind != JsonValueKind.String)
            throw new InvalidOperationException("Operator must be string");

        var opString = element.Value.GetString();

        // Unsupported operator type
        if (!Enum.TryParse<OperationType>(opString, true, out var opEnum))
            throw new InvalidOperationException("Operator not supported");

        return opEnum;
    }

    private double ParseOperand(JsonElement? element, int key)
    {
        // Check if exists
        if (element == null || element.Value.ValueKind == JsonValueKind.Null)
            throw new InvalidOperationException($"Operand {key} is missing");

        switch (element.Value.ValueKind)
        {
            case JsonValueKind.Number:
                return element.Value.GetDouble();

            case JsonValueKind.String:
                // Parse to double
                if (!double.TryParse(
                    element.Value.GetString(),
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var value))
                {
                    throw new InvalidOperationException($"Operand {key} is invalid");
                }
                return value;

            default:
                throw new InvalidOperationException($"Operand {key} is invalid");
        }
    }
}
