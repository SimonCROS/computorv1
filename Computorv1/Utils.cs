namespace Computorv1;

using Computorv1.Nodes;

public static class Utils
{
    public static Node Simplify(Node node)
    {
        if (node is EqualNode equal)
            return Simplify(new SubNode(Simplify(equal.Left), Simplify(equal.Right)));
        if (node is AddNode add)
            return (Simplify(add.Left), Simplify(add.Right)) switch
            {
                (NumberNode left, NumberNode right) => new NumberNode(left.Value + right.Value),
                (NumberNode left, Node right) when left.Value == 0 => right,
                (Node left, NumberNode right) when right.Value == 0 => left,
                (Node left, Node right) => new AddNode(left, right)
            };
        if (node is SubNode sub)
            return (Simplify(sub.Left), Simplify(sub.Right)) switch
            {
                (NumberNode left, NumberNode right) => new NumberNode(left.Value - right.Value),
                (Node left, NumberNode right) when right.Value == 0 => left,
                (Node left, Node right) when left == right => new NumberNode(0),
                (Node left, Node right) => new SubNode(left, right)
            };
        if (node is MulNode mul)
            return (Simplify(mul.Left), Simplify(mul.Right)) switch
            {
                (NumberNode left, NumberNode right) => new NumberNode(left.Value * right.Value),
                (NumberNode left, Node other) when left.Value == 1 => other,
                (Node other, NumberNode right) when right.Value == 1 => other,
                (NumberNode left, _) when left.Value == 0 => new NumberNode(0),
                (_, NumberNode right) when right.Value == 0 => new NumberNode(0),
                (Node left, PowNode right) when left == right.Left && right.Right is NumberNode exp => new PowNode(left, new NumberNode(exp.Value + 1)),
                (PowNode left, Node right) when right == left.Left && left.Right is NumberNode exp => new PowNode(right, new NumberNode(exp.Value + 1)),
                (Node left, Node right) when left == right => new PowNode(left, new NumberNode(2)),
                (NumberNode left, MulNode right) when right.Left is NumberNode childNumber => new MulNode(new NumberNode(left.Value * childNumber.Value), right.Right),
                (NumberNode left, MulNode right) when right.Right is NumberNode childNumber => new MulNode(new NumberNode(left.Value * childNumber.Value), right.Left),
                (MulNode left, NumberNode right) when left.Left is NumberNode childNumber => new MulNode(new NumberNode(right.Value * childNumber.Value), left.Right),
                (MulNode left, NumberNode right) when left.Right is NumberNode childNumber => new MulNode(new NumberNode(right.Value * childNumber.Value), left.Left),
                (Node left, NumberNode right) => new MulNode(right, left),
                (Node left, Node right) => new MulNode(left, right)
            };
        if (node is PowNode pow)
            return (Simplify(pow.Left), Simplify(pow.Right)) switch
            {
                (NumberNode left, NumberNode right) when float.IsInteger(right.Value) && float.IsPositive(right.Value) => new NumberNode(MyMathF.Pow(left.Value, (uint)right.Value)),
                (NumberNode left, Node other) when left.Value == 1 => other,
                (Node other, NumberNode right) when right.Value == 1 => other,
                (NumberNode left, _) when left.Value == 0 => new NumberNode(1),
                (_, NumberNode right) when right.Value == 0 => new NumberNode(1),
                (Node left, Node right) => new PowNode(left, right)
            };
        return node;
    }

    public static List<Monominal> ListMonominals(Node node)
    {
        List<Monominal> raw = new();
        ListMonominalsRec(node, raw);

        Dictionary<float, Monominal> monominalsByExponent = new();
        foreach (Monominal monominal in raw)
        {
            if (monominalsByExponent.TryGetValue(monominal.Exponent, out Monominal existingMonominal))
                monominalsByExponent[monominal.Exponent] = new Monominal(existingMonominal.Coefficient + monominal.Coefficient, monominal.Identifier, monominal.Exponent);
            else
                monominalsByExponent.Add(monominal.Exponent, monominal);
        }

        List<Monominal> monominals = monominalsByExponent.Values.Where(m => m.Coefficient != 0).ToList();
        monominals.Sort();
        return monominals;
    }

    private static void ListMonominalsRec(Node current, List<Monominal> monominals, float sign = 1)
    {
        if (current is AddNode or SubNode)
        {
            ListMonominalsRec(((BinaryOperatorNode)current).Left, monominals, sign);
            ListMonominalsRec(((BinaryOperatorNode)current).Right, monominals, current is SubNode ? -sign : sign);
        }
        else if (current is MulNode mul)
        {
            NumberNode coefficient = (NumberNode)mul.Left;
            if (mul.Right is PowNode powNode)
            {
                monominals.Add(new Monominal(coefficient.Value * sign, ((IdentifierNode)powNode.Left).Value, (int)((NumberNode)powNode.Right).Value));
            }
            else if (mul.Right is IdentifierNode identifierNode)
            {
                monominals.Add(new Monominal(coefficient.Value * sign, identifierNode.Value, 1));
            }
        }
        else if (current is PowNode powNode)
        {
            monominals.Add(new Monominal(1 * sign, ((IdentifierNode)powNode.Left).Value, (int)((NumberNode)powNode.Right).Value));
        }
        else if (current is IdentifierNode identifierNode)
        {
            monominals.Add(new Monominal(1 * sign, identifierNode.Value, 1));
        }
        else if (current is NumberNode numberNode)
        {
            monominals.Add(new Monominal(numberNode.Value * sign, string.Empty, 0));
        }
        else
        {
            throw new NotSupportedException(current.GetType().Name + " should not be present here");
        }
    }
}
