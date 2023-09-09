namespace Computorv1;

using System.Text;

public record struct Monominal(float Coefficient, string Identifier, int Exponent) : IComparable<Monominal>
{
    public readonly bool IsNegative => float.IsNegative(Coefficient);

    public Monominal(float coefficient)
        : this(coefficient, string.Empty, 0)
    {
    }

    public Monominal(float coefficient, string identifier)
        : this(coefficient, identifier, 1)
    {
    }

    public readonly Monominal Negate()
    {
        return new Monominal(-Coefficient, Identifier, Exponent);
    }

    public override readonly string ToString()
    {
        StringBuilder sb = new();

        if (Coefficient == 0)
            return "0";
        if (Exponent == 0)
            return Coefficient.ToString();

        if (Coefficient != 1)
            sb.Append($"{Coefficient} * ");

        if (Exponent == 1)
            sb.Append(Identifier);
        else
            sb.Append($"{Identifier}^{Exponent}");

        return sb.ToString();
    }

    public readonly int CompareTo(Monominal other)
    {
        if (Exponent == other.Exponent)
            return 0;
        return Exponent > other.Exponent ? -1 : 1;
    }
}
