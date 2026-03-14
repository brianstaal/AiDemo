using System.Text.Json.Serialization;
namespace AiDemo;

public class Input(string type = "text")
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;

    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Content { get; set; }

    [JsonPropertyName("data_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DataUrl { get; set; }
}
