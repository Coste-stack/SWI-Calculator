
using System.Text.Json.Serialization;

public class Operation
{
    [JsonPropertyName("operator")]
    public string? Operator { get; set; }

    [JsonPropertyName("value1")]
    public double? Value1 { get; set; }
    
    [JsonPropertyName("value2")]
    public double? Value2 { get; set; }

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