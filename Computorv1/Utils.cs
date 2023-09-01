namespace computorv1;

using computorv1.Nodes;

public static class Utils
{
    public static Node Simplify(Node node)
    {
        if (node is EqualNode equal)
            return new EqualNode(Simplify(equal.Left), Simplify(equal.Right));
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
                (Node left, Node right) => new MulNode(left, right)
            };
        if (node is PowNode pow)
            return (Simplify(pow.Left), Simplify(pow.Right)) switch
            {
                (NumberNode left, NumberNode right) => new NumberNode(MathF.Pow(left.Value, right.Value)),
                (NumberNode left, Node other) when left.Value == 1 => other,
                (Node other, NumberNode right) when right.Value == 1 => other,
                (NumberNode left, _) when left.Value == 0 => new NumberNode(1),
                (_, NumberNode right) when right.Value == 0 => new NumberNode(1),
                (Node left, Node right) => new PowNode(left, right)
            };
        return node;
    }
}
