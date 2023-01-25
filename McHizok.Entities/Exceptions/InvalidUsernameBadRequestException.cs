namespace McHizok.Entities.Exceptions;

public class InvalidUsernameBadRequestException : BadRequestException
{
    public InvalidUsernameBadRequestException() : base("The username must consist of the hungarian alphabet, numbers and following symbols: -._@+")
    {
    }
}
