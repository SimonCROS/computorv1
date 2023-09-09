namespace Computorv1.Exceptions;

[Serializable]
public class ParsingException : Exception
{
    public ParsingException(string message) : base(message) { }
    public ParsingException(string message, Exception inner) : base(message, inner) { }
}
