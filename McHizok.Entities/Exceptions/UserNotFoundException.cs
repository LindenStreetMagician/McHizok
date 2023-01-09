namespace McHizok.Entities.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string userId) 
        : base($"The user with id: {userId} doesn't exist in the database")
    {
    }
}
