using System.Text.Json.Serialization;

namespace AiDemo;

public class Instruction
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("input")]
    public List<Input> Inputs { get; set; } = [];

    [JsonPropertyName("system_prompt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SystemPrompt { get; set; }

    [JsonPropertyName("context_length")]
    public int ContextLength { get; set; }

    public Instruction(string model, string input, int contextlength = 8000)
    { 
        Model = model;
        ContextLength = contextlength;
        var inp = new Input() {Content = input};
        Inputs.Add(inp);
    }

    public async Task AddAttachment(string attachment)
    {
        var attachmentExtention = Path.GetExtension(attachment);

        var mimeType = attachmentExtention switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".txt" => "text/plain",
            ".json" => "application/json",
            ".pdf" => "application/pdf",
            _ => throw new NotSupportedException($"Attachment type {attachmentExtention} is not supported.")
        };

        if (mimeType.StartsWith("text") || mimeType.IndexOf("json", StringComparison.InvariantCultureIgnoreCase) >= 0)
        {
            var content = File.ReadAllText(attachment);
            var inputText = new Input("text") { Content = content };
            Inputs.Add(inputText);
            return;
        }

        var base64Content = Convert.ToBase64String(File.ReadAllBytes(attachment));
        var base64Attachment = $"data:{mimeType};base64,{base64Content}";
        var inp = new Input("image") { DataUrl = base64Attachment };
        Inputs.Add(inp);
    }
}
