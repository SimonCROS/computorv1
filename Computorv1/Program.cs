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
            foreach (Monominal monominal in ListMonominals(lhs))
            {
                Console.WriteLine($" - {monominal}");
            }
            Console.WriteLine("RHS:");
            foreach (Monominal monominal in ListMonominals(rhs))
            {
                Console.WriteLine($" - {monominal}");
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
        NumberNode coefficient = (NumberNode)mul.Left;
        if (mul.Right is PowNode pow)
        {
            if (pow.Left is IdentifierNode identifierNode && pow.Right is NumberNode exponent)
            {
                monominals.Add(new Monominal(coefficient.Value, identifierNode.Value, exponent.Value));
            }
        }
        else if (mul.Right is IdentifierNode identifierNode)
        {
            monominals.Add(new Monominal(coefficient.Value, identifierNode.Value, 1));
        }
    }
    else if (current is IdentifierNode identifierNode)
    {
        monominals.Add(new Monominal(1, identifierNode.Value, 1));
    }
    else if (current is NumberNode numberNode)
    {
        monominals.Add(new Monominal(numberNode.Value, "", 0));
    }
}

record struct Monominal(float Coefficient, string Identifier, float Exponent);
