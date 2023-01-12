using System.Text.Json.Serialization;

namespace McHizok.Entities.Models.InputForm;

public class Answer
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("xpath")]
    public string XPath { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("text")]
    public List<string> Text { get; set; }
}
