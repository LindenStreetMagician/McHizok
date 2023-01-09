using System.Text.Json.Serialization;

namespace McHizok.Entities.Models.InputForm;

public class Question
{
    [JsonPropertyName("questionText")]
    public string QuestionText { get; set; }

    [JsonPropertyName("isRandomAnswer")]
    public bool IsRandomAnswer { get; set; }

    [JsonPropertyName("answers")]
    public List<Answer> Answers { get; set; }
}
