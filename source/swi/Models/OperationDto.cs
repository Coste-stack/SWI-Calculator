using System.Text.Json.Serialization;

public class OperationDto
{
    [JsonPropertyName("operator")]
    public object? Operator { get; set; }

    [JsonPropertyName("value1")]
    public object? Value1 { get; set; }
    
    [JsonPropertyName("value2")]
    public object? Value2 { get; set; }
}