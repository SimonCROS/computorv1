namespace Computorv1.Nodes;

using System.Text;

public abstract record class Node;

public abstract record class OperatorNode(int Precedence) : Node
{
}

// public abstract record class UnaryOperatorNode(Node Child, char Symbol, int Precedence) : OperatorNode(Precedence)
// {
//     public sealed override string ToString()
//     {
//         if (Child is OperatorNode c && c.Precedence > Precedence)
//             return $"{Symbol}({Child})";
//         else if (Child is BinaryOperatorNode)
//             return $"{Symbol}({Child})";
//         else
//             return $"{Symbol}{Child}";
//     }
// }

// public record class NegateNode(Node Child) : UnaryOperatorNode(Child, '-', 2);

public abstract record class BinaryOperatorNode(Node Left, Node Right, char Symbol, int Precedence) : OperatorNode(Precedence)
{
    public sealed override string ToString()
    {
        StringBuilder result = new();

        if (Left is OperatorNode l && l.Precedence > Precedence)
            result.Append($"({Left})");
        else
            result.Append($"{Left}");

        result.Append($" {Symbol} ");

        if (Right is OperatorNode r && r.Precedence > Precedence)
            result.Append($"({Right})");
        else
            result.Append($"{Right}");

        return result.ToString();
    }
}

public record class AddNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '+', 4);

public record class SubNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '-', 4);

public record class MulNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '*', 3);

public record class PowNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '^', 2);

public record class EqualNode(Node Left, Node Right) : BinaryOperatorNode(Left, Right, '=', 14);

public record class IdentifierNode(string Value) : Node
{
    public override string ToString() => $"{Value}";
}

public record class NumberNode(float Value) : Node
{
    public override string ToString() => $"{Value}";
}
