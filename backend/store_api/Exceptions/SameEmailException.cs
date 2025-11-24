namespace store_api.Exceptions;

public class SameEmailException : Exception
{

    public SameEmailException()
    {
        
    }

    public SameEmailException(string message) : base(message)
    {
        
    }

    public SameEmailException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
    
}