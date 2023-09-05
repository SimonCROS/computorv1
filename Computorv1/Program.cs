using System.Diagnostics;
using System.Globalization;
using computorv1;
using computorv1.Nodes;
using computorv1.Tokens;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

if (args.Length != 1)
{
    Console.WriteLine("Usage: dotnet run <equation>");
    return 1;
}

if (new Lexer(args[0]).Tokenize(out List<Token> tokens))
{
    if (new Parser(tokens).Parse(out Node? node))
    {
        EqualNode equalNode = (EqualNode)node;

        Node standard = Utils.Simplify(new SubNode(equalNode.Left, equalNode.Right));
        if (new Validator(maxIdentifiersCount: 1).Validate(standard))
        {
            Console.WriteLine($"Reduced form: {standard}");

            Dictionary<float, Monominal> monominalsByExponent = new();
            foreach (Monominal monominal in Utils.ListMonominals(standard))
            {
                if (monominalsByExponent.TryGetValue(monominal.Exponent, out Monominal existingMonominal))
                {
                    float newCoefficient = existingMonominal.Coefficient + monominal.Coefficient;
                    if (newCoefficient == 0)
                    {
                        monominalsByExponent.Remove(monominal.Exponent);
                        continue;
                    }

                    monominalsByExponent[monominal.Exponent] = new Monominal(newCoefficient, monominal.Identifier, monominal.Exponent);
                }
                else
                {
                    monominalsByExponent.Add(monominal.Exponent, monominal);
                }
            }

            List<Monominal> monominals = monominalsByExponent.Values.ToList();
            monominals.Sort();

            float degree = monominals.Count == 0 ? 0 : monominals.Max(m => m.Exponent);
            if (degree > 2)
            {
                Console.WriteLine("The polynomial degree is strictly greater than 2, I can't solve.");
                return 1;
            }

            Console.WriteLine($"Monominals small: {string.Join(" | ", monominals)}");
            Console.WriteLine($"Polynomial degree: {degree}");
        }
        else
        {
            return 1;
        }
    }
    else
    {
        return 1;
    }
}
else
{
    return 1;
}

return 0;
