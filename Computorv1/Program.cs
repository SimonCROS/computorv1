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
        node = Utils.Simplify(node);
        if (new Validator(maxIdentifiersCount: 1).Validate(node))
        {
            Console.WriteLine(node);

            Node lhs = ((EqualNode)node).Left;
            Node rhs = ((EqualNode)node).Right;
            Console.WriteLine("LHS:");
            foreach (Monominal monominal in Utils.ListMonominals(lhs))
            {
                Console.Write($"{monominal} ");
            }
            Console.WriteLine();
            Console.WriteLine("RHS:");
            foreach (Monominal monominal in Utils.ListMonominals(rhs))
            {
                Console.Write($"{monominal} ");
            }
            Console.WriteLine();
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
