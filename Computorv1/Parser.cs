namespace computorv1;

using System.Diagnostics.CodeAnalysis;
using computorv1.Collections;
using computorv1.Nodes;
using computorv1.Tokens;

public class Parser
{
    private readonly TokenEnumerator _tokens;

    public Parser(IReadOnlyList<Token> tokens)
    {
        _tokens = new(tokens);
    }

    public bool Parse([MaybeNullWhen(false)] out Node result)
    {
        result = null;
        
        bool ret = Assignation(out EqualNode? equal);
        if (_tokens.MoveNext() && _tokens.Current is not null)
        {
            Console.WriteLine($"Unexpected token: {_tokens.Current}");
            return false;
        }
        if (!ret)
        {
            Console.WriteLine($"Unexpected end of file");
            return false;
        }
        result = equal!;
        return true;
    }

    public bool Assignation([MaybeNullWhen(false)] out EqualNode result)
    {
        result = null;
        if (!Expr(out Node? lhs))
            return false;

        // Don't do loop so there is at most one assignation
        Token? token = _tokens.Peek();
        if (token is EqualToken)
        {
            _tokens.MoveNext();
            if (Expr(out Node? rhs))
                result = new EqualNode(lhs, rhs);
            else
                return false;
        }
        else
        {
            return false; // Force at least one assignation
        }

        return true;
    }

    public bool Expr([MaybeNullWhen(false)] out Node result)
    {
        if (!Factor(out result))
            return false;

        Token? token;
        while ((token = _tokens.Peek()) is not null)
        {
            if (token is AddToken)
            {
                _tokens.MoveNext();
                if (Factor(out Node? rhs))
                    result = new AddNode(result, rhs);
                else
                    return false;
            }
            else if (token is SubToken)
            {
                _tokens.MoveNext();
                if (Factor(out Node? rhs))
                    result = new SubNode(result, rhs);
                else
                    return false;
            }
            else
                break;
        }

        return true;
    }

    public bool Factor([MaybeNullWhen(false)] out Node result)
    {
        if (!Pow(out result))
            return false;

        Token? token;
        while ((token = _tokens.Peek()) is not null)
        {
            if (token is MulToken)
            {
                _tokens.MoveNext();
                if (Pow(out Node? rhs))
                    result = new MulNode(result, rhs);
                else
                    return false;
            }
            else
                break;
        }

        return true;
    }

    public bool Pow([MaybeNullWhen(false)] out Node result)
    {
        if (!Term(out result))
            return false;

        Token? token;
        while ((token = _tokens.Peek()) is not null)
        {
            if (token is PowToken)
            {
                _tokens.MoveNext();
                if (Term(out Node? rhs))
                    result = new PowNode(result, rhs);
                else
                    return false;
            }
            else
                break;
        }

        return true;
    }

    public bool Term([MaybeNullWhen(false)] out Node result)
    {
        result = null;

        Token? token = _tokens.Peek();
        if (token is NumberToken numToken)
        {
            _tokens.MoveNext();
            result = new NumberNode(numToken.Value);
        }
        else if (token is IdentifierToken idToken)
        {
            _tokens.MoveNext();
            result = new IdentifierNode(idToken.Value);
        }
        else if (token is SubToken)
        {
            _tokens.MoveNext();

            // Implement only if content is a number, and do not use NegateNode
            if (Term(out Node? next) && next is NumberNode childNumberNode)
                result = new NumberNode(-childNumberNode.Value);
            else
                return false;
        }
        else
            return false;

        return true;
    }
}
