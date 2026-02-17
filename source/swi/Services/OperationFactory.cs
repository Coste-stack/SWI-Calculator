using System.Text.Json;

public class OperationFactory : IOperationFactory
{
    private readonly Dictionary<OperationType, IOperationStrategy> _strategyMap;

    public OperationFactory(IEnumerable<IOperationStrategy> strategies)
    {
        _strategyMap = strategies.ToDictionary(s => s.SupportedOperator);
    }

    // Map dto to operation model and validate
    public Operation Create(OperationDto dto)
    {
        var operatorType = ParseOperator(dto.Operator);

        // Get strategy for min/max operands numbers
        if (!_strategyMap.TryGetValue(operatorType, out var strategy))
            throw new InvalidOperationException($"No strategy found for operator '{operatorType}'");

        var operands = new List<double>();
        var operandElements = new JsonElement?[] { dto.Value1, dto.Value2 };
        // Parse operands and check if they match min/max op numbers
        for (int i = 0; i < operandElements.Length; i++)
        {
            var element = operandElements[i];

            // Element missing
            if (!element.HasValue || element.Value.ValueKind == JsonValueKind.Null)
            {
                // Not enough operands 
                if (operands.Count < strategy.MinOperands)
                {
                    throw new InvalidOperationException($"Operand {i + 1} is missing");
                }
                else
                {
                    break;
                }
            }

            // Enough operands
            if (strategy.MaxOperands.HasValue && operands.Count >= strategy.MaxOperands.Value)
                break;
            
            // Parse and validate
            operands.Add(ParseOperand(element, i + 1));
        }

        return new Operation(operatorType, operands);
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
