using System.Text.Json;
using System.Text.Json.Serialization;

public class OperationDto
{
    [JsonPropertyName("operator")]
    public JsonElement? Operator { get; set; }

    [JsonPropertyName("value1")]
    public JsonElement? Value1 { get; set; }
    
    [JsonPropertyName("value2")]
    public JsonElement? Value2 { get; set; }
}