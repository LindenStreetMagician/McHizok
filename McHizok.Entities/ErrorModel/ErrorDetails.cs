using System.Text.Json;

namespace McHizok.Entities.ErrorModel;

public class ErrorDetails
{
	private JsonSerializerOptions _serializerOptions = new();

	public ErrorDetails()
	{
        _serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

    }

	public int StatusCode { get; set; }
	public string? Message { get; set; }

	public override string ToString() => JsonSerializer.Serialize(this, _serializerOptions);
}
