namespace computorv1.Nodes;

public abstract record class Node;

// public abstract record class UnaryOperatorNode(Node Child, char Symbol) : Node
// {
//     public sealed override string ToString() => $"({Symbol}{Child})";
// }

public abstract record class BinaryOperatorNode(Node Left, Node Right, char Symbol) : Node
{
    public sealed override string ToString() => $"({Left} {Symbol} {Right})";
}

// public record class NegateNode(Node Child) : UnaryOperatorNode(Child, '-');

public record class AddNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '+');

public record class SubNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '-');

public record class MulNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '*');

public record class PowNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '^');

public record class EqualNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '=');

public record class IdentifierNode(string Value) : Node
{
    public override string ToString() => $"{Value}";
}

public record class NumberNode(float Value) : Node
{
    public override string ToString() => $"{Value}";
}
