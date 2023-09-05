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
            List<Monominal> monominals = Utils.ListMonominals(standard);
            monominals.Sort();

            Console.WriteLine($"Std form: {standard}");
            Console.WriteLine($"Monominals: {string.Join(" | ", monominals)}");
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
