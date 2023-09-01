namespace computorv1;

using computorv1.Nodes;

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
            InternalValidate(node, 0, false, false);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    private void InternalValidate(Node node, int level, bool inMul, bool exponent)
    {
        switch (node)
        {
            case EqualNode equalNode:
                if (level != 0)
                    throw new Exception("Invalid expression: = should be on the top level");
                InternalValidate(equalNode.Left, level + 1, inMul, exponent);
                InternalValidate(equalNode.Right, level + 1, inMul, exponent);
                break;
            case AddNode or SubNode:
                if (inMul || exponent)
                    throw new Exception("Invalid expression: non constant nested + or -");
                InternalValidate(((BinaryOperatorNode)node).Left, level + 1, inMul, exponent);
                InternalValidate(((BinaryOperatorNode)node).Right, level + 1, inMul, exponent);
                break;
            case MulNode mulNode:
                if (inMul || exponent)
                    throw new Exception("Invalid expression: non constant nested multiplication");
                InternalValidate(mulNode.Left, level + 1, true, exponent);
                InternalValidate(mulNode.Right, level + 1, true, exponent);
                break;
            case PowNode powNode:
                if (exponent)
                    throw new Exception("Invalid expression: non constant nested pow");
                InternalValidate(powNode.Left, level + 1, inMul, exponent);
                InternalValidate(powNode.Right, level + 1, inMul, true);
                break;
            case IdentifierNode identifierNode:
                if (exponent)
                    throw new Exception($"Invalid expression: cannot use {identifierNode.Value} as exponent");
                if (!_identifiers.Contains(identifierNode.Value))
                {
                    if (_identifiers.Count >= _maxIdentifiersCount)
                        throw new Exception($"Invalid expression: too many identifiers (max {_maxIdentifiersCount})");
                    else
                        _identifiers.Add(identifierNode.Value);
                }
                break;
            default:
                return;
        }
    }
}
