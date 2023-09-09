namespace Computorv1;

using Computorv1.Nodes;

public class Validator
{
    private readonly int _maxIdentifiersCount;
    private readonly List<string> _identifiers;

    public Validator(int maxIdentifiersCount)
    {
        _maxIdentifiersCount = maxIdentifiersCount;
        _identifiers = new List<string>(maxIdentifiersCount);
    }

    public bool Validate(Node node)
    {
        _identifiers.Clear();
        try
        {
            InternalValidate(node, 0, false);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    private void InternalValidate(Node node, int level, bool inMul)
    {
        switch (node)
        {
            case EqualNode equalNode:
                if (level != 0)
                    throw new Exception("= should be on the top level");
                InternalValidate(equalNode.Left, level + 1, inMul);
                InternalValidate(equalNode.Right, level + 1, inMul);
                break;
            case AddNode or SubNode:
                if (inMul)
                    throw new Exception("addition or subtraction should not be inside a multiplication");
                InternalValidate(((BinaryOperatorNode)node).Left, level + 1, inMul);
                InternalValidate(((BinaryOperatorNode)node).Right, level + 1, inMul);
                break;
            case MulNode mulNode:
                if (inMul)
                    throw new Exception("non constant nested multiplication");
                InternalValidate(mulNode.Left, level + 1, true);
                InternalValidate(mulNode.Right, level + 1, true);
                break;
            case PowNode powNode:
                if (powNode.Right is not NumberNode)
                    throw new Exception("a const number must be used as exponent");
                if (!float.IsInteger(((NumberNode)powNode.Right).Value))
                    throw new Exception("cannot use a floating number as exponent");
                InternalValidate(powNode.Left, level + 1, inMul);
                InternalValidate(powNode.Right, level + 1, inMul);
                break;
            case IdentifierNode identifierNode:
                if (!_identifiers.Contains(identifierNode.Value))
                {
                    if (_identifiers.Count >= _maxIdentifiersCount)
                        throw new Exception($"too many identifiers (max {_maxIdentifiersCount})");
                    else
                        _identifiers.Add(identifierNode.Value);
                }
                break;
            default:
                return;
        }
    }
}
