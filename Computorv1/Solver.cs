namespace Computorv1;

using System.Text;
using Computorv1.Nodes;
using Computorv1.Tokens;

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

        List<Monominal> monominals = Utils.ListMonominals(standard);

        Console.WriteLine($"Reduced form: {GetReducedForm(monominals)} = 0");

        return Solve(monominals);
    }

    public static string GetReducedForm(List<Monominal> monominals)
    {
        StringBuilder sb = new();
        foreach (Monominal monominal in monominals)
        {
            if (monominal.Coefficient == 0)
                continue;

            if (sb.Length > 0)
            {
                if (!monominal.IsNegative)
                    sb.Append($" + {monominal}");
                else
                    sb.Append($" - {monominal.Negate()}");
            }
            else
            {
                sb.Append(monominal);
            }
        }

        return sb.Length == 0 ? "0" : sb.ToString();
    }

    private static int Solve(List<Monominal> monominals)
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
                float sqrtDiscriminant = MyMathF.Sqrt(discriminant);
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
                float sqrtDiscriminant = MyMathF.Sqrt(-discriminant);

                Console.WriteLine($"Discriminant is strictly negative, the two solutions are:");
                Console.WriteLine($"{-b / (2 * a)} + {sqrtDiscriminant / (2 * a)}i");
                Console.WriteLine($"{-b / (2 * a)} - {sqrtDiscriminant / (2 * a)}i");
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
