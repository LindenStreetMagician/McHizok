namespace McHizok.Entities.Exceptions;

public class BlockCodeInvalidBadRequestException : BadRequestException
{
    public BlockCodeInvalidBadRequestException(string message)
        : base(message)
    {
    }
}
