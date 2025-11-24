namespace store_api.Exceptions;

public class InvalidDtoType : Exception
{

    public InvalidDtoType()
    {
        
    }

    public InvalidDtoType(string message) : base(message)
    {
        
    }

    public InvalidDtoType(string message, Exception inner) : base(message, inner)
    {
        
    }
    
}