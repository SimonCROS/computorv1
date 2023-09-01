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
    if (new Parser(tokens).Parse(out EqualNode? result))
    {
        Console.WriteLine(result);
        Node lhs = Utils.Simplify(result.Left);
        Node rhs = Utils.Simplify(result.Right);
        Console.WriteLine($"({lhs} = {rhs})");
        ListMonominals(result.Right);
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

List<Monominal> ListMonominals(Node node)
{
    List<Monominal> monominals = new();
    ListMonominalsRec(node, monominals);
    return monominals;
}

void ListMonominalsRec(Node current, List<Monominal> monominals)
{
    if (current is AddNode or SubNode)
    {
        ListMonominalsRec(((BinaryOperatorNode)current).Left, monominals);
        ListMonominalsRec(((BinaryOperatorNode)current).Right, monominals);
    }
    else if (current is MulNode mul)
    {
        if (mul.Left is NumberNode coefficient && mul.Right is PowNode pow)
        {
            if (pow.Left is IdentifierNode identifier && pow.Right is NumberNode exponent)
            {
                monominals.Add(new Monominal(coefficient.Value, exponent.Value));
            }
        }
    }
}

record struct Monominal(float Coefficient, float Exponent);
