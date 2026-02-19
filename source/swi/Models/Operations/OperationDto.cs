using System.Text.Json;
using System.Text.Json.Serialization;

namespace swi.Models.Operations;

public class OperationDto
{
    [JsonPropertyName("operator")]
    public JsonElement? Operator { get; set; }

    [JsonPropertyName("value1")]
    public JsonElement? Value1 { get; set; }

    [JsonPropertyName("value2")]
    public JsonElement? Value2 { get; set; }

    public override string ToString()
    {
        return $"Operator: {Format(Operator)}, " +
               $"Value1: {Format(Value1)}, " +
               $"Value2: {Format(Value2)}";
    }

    private static string Format(JsonElement? element)
    {
        if (element == null)
            return "null";

        var value = element.Value;

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString() ?? "null",
            JsonValueKind.Number => value.GetRawText(),
            JsonValueKind.Null => "null",
            _ => value.GetRawText()
        };
    }
}