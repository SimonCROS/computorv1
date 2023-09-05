namespace computorv1;

public record struct Monominal(float Coefficient, string Identifier, int Exponent) : IComparable<Monominal>
{
    public Monominal(float coefficient)
        : this(coefficient, string.Empty, 0)
    {
    }

    public Monominal(float coefficient, string identifier)
        : this(coefficient, identifier, 1)
    {
    }

    public override readonly string ToString()
    {
        if (Exponent == 0)
            return $"{Coefficient}";
        if (Exponent == 1)
            return $"{Coefficient}{Identifier}";
        return $"{Coefficient }{Identifier} ^ {Exponent}";
    }

    public readonly int CompareTo(Monominal other)
    {
        if (Exponent == other.Exponent)
            return 0;
        return Exponent > other.Exponent ? -1 : 1;
    }
}
