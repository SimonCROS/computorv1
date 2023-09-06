using computorv1.Nodes;
using computorv1.Tokens;

namespace computorv1;

public static class Solver
{
    public static int Solve(string equation)
    {
        if (!new Lexer(equation).Tokenize(out List<Token> tokens))
            return 1;

        if (!new Parser(tokens).Parse(out Node? node))
            return 1;

        Node standard = Utils.Simplify(node);

        if (!new Validator(maxIdentifiersCount: 1).Validate(standard))
            return 1;

        Console.WriteLine($"Reduced form: {standard} = 0");

        List<Monominal> monominals = Utils.ListMonominals(standard);
        return Solve(monominals);
    }

    public static int Solve(List<Monominal> monominals)
    {
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

        return 0;
    }
}
