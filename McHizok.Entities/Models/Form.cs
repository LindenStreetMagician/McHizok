using System.Text.Json.Serialization;

namespace McHizok.Entities.Models;

public class Form
{
    [JsonPropertyName("formUrl")]
    public string FormUrl { get; set; }

    [JsonPropertyName("promoCodeInputXPath")]
    public string PromoCodeInputXPath { get; set; }

    [JsonPropertyName("sendPromoCodeButtonXPath")]
    public string SendPromoCodeButtonXPath { get; set; }

    [JsonPropertyName("firstAnswerXPath")]
    public string FirstAnswerXPath { get; set; }

    [JsonPropertyName("submitButtonXPath")]
    public string SubmitButtonXPath { get; set; }

    [JsonPropertyName("codeInvalidBlockXPath")]
    public string CodeInvalidBlockXPath { get; set; }

    [JsonPropertyName("couponXPath")]
    public string CouponXPath { get; set; }

    [JsonPropertyName("form")]
    public List<Question> Questions { get; set; }
}
