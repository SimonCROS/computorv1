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
                    monominalsByExponent[monominal.Exponent] = new Monominal(existingMonominal.Coefficient + monominal.Coefficient, monominal.Identifier, monominal.Exponent);
                else
                    monominalsByExponent.Add(monominal.Exponent, monominal);
            }

            List<Monominal> monominals = monominalsByExponent.Values.Where(m => m.Coefficient != 0).ToList();
            monominals.Sort();

            float degree = monominals.Count == 0 ? 0 : monominals.Max(m => m.Exponent);
            Console.WriteLine($"Polynomial degree: {degree}");
            if (degree > 2)
            {
                Console.WriteLine("The polynomial degree is strictly greater than 2, I can't solve.");
                return 1;
            }

            if (degree == 2)
            {
                float a = monominals.FirstOrDefault(m => m.Exponent == 2).Coefficient;
                float b = monominals.FirstOrDefault(m => m.Exponent == 1).Coefficient;
                float c = monominals.FirstOrDefault(m => m.Exponent == 0).Coefficient;

                float discriminant = b * b - 4 * a * c;

                if (discriminant > 0)
                {
                    float sqrtDiscriminant = MathF.Sqrt(discriminant);
                    float solution1 = (-b - sqrtDiscriminant) / (2 * a);
                    float solution2 = (-b + sqrtDiscriminant) / (2 * a);

                    Console.WriteLine($"Discriminant is strictly positive, the two solutions are:");
                    Console.WriteLine($"{solution1}");
                    Console.WriteLine($"{solution2}");
                }
                else if (discriminant == 0)
                {
                    Console.WriteLine($"The solution is:");
                    Console.WriteLine($"{-b / (2 * a)}");
                }
                else
                {
                    float sqrtDiscriminant = MathF.Sqrt(-discriminant);
                    
                    Console.WriteLine($"Discriminant is strictly negative, the two solutions are:");
                    Console.WriteLine($"{-b / (2 * a)} - {sqrtDiscriminant / (2 * a)}i");
                    Console.WriteLine($"{-b / (2 * a)} + {sqrtDiscriminant / (2 * a)}i");
                }
            }
            else if (degree == 1)
            {
                float a = monominals.FirstOrDefault(m => m.Exponent == 1).Coefficient;
                float b = monominals.FirstOrDefault(m => m.Exponent == 0).Coefficient;
                Console.WriteLine($"The solution is:");
                Console.WriteLine($"{-b / a}");
            }
            else if (monominals.Count == 0)
            {
                Console.WriteLine("All real numbers are solution");
            }
            else
            {
                Console.WriteLine("No solution");
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
}
else
{
    return 1;
}

return 0;
