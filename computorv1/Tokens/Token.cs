namespace computorv1.Tokens
{
    public abstract record Token;

    public record class AddToken() : Token;

    public record class SubToken() : Token;

    public record class MulToken() : Token;

    public record class PowToken() : Token;

    public record class EqualToken() : Token;

    public record class IdentifierToken(string Identifier) : Token;

    public record class NumberToken(float Number) : Token;
}
