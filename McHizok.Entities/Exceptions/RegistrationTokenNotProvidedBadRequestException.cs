namespace McHizok.Entities.Exceptions;

public class RegistrationTokenNotProvidedBadRequestException : BadRequestException
{
	public RegistrationTokenNotProvidedBadRequestException()
		: base("The provided token was invalid")
	{
	}
}
