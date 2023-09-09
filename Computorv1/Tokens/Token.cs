namespace Computorv1.Tokens;

public abstract record class Token
{
    private readonly string _str;

    protected Token(string stringRepresentation)
    {
        _str = stringRepresentation;
    }

    public sealed override string ToString() => _str;
}

public abstract record class ComplexToken : Token
{
    protected ComplexToken(string stringRepresentation)
        : base(stringRepresentation)
    {
    }
}

public record class AddToken() : Token("+");

public record class SubToken() : Token("-");

public record class MulToken() : Token("*");

public record class PowToken() : Token("^");

public record class EqualToken() : Token("=");

public record class IdentifierToken(string Value) : ComplexToken(Value);

public record class NumberToken(float Value) : ComplexToken(Value.ToString());
