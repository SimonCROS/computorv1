﻿namespace Computorv1;

using Computorv1.Collections;
using Computorv1.Exceptions;
using Computorv1.Tokens;

public readonly struct Lexer
{
    private readonly PeekableEnumerator<char> _chars;

    public Lexer(string str)
    {
        _chars = new(str.ToList());
    }

    public bool Tokenize(out List<Token> tokens)
    {
        tokens = new();

        try
        {
            while (NextToken(out Token token))
            {
                tokens.Add(token);
            }
        }
        catch (ParsingException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        return true;
    }

    private bool NextToken(out Token token)
    {
        while (_chars.MoveNextIf(char.IsWhiteSpace))
        {
        }
        char c = _chars.Peek();
        token = c switch
        {
            '+' => new AddToken(),
            '-' => new SubToken(),
            '*' => new MulToken(),
            '^' => new PowToken(),
            '=' => new EqualToken(),
            '\0' => null!,
            _ => NextComplexToken(),
        };
        if (token is not ComplexToken)
        {
            _chars.MoveNext();
        }
        return token != null;
    }

    private Token NextComplexToken()
    {
        if (ReadIdentifier(out string identifier))
        {
            return new IdentifierToken(identifier);
        }
        if (ReadNumber(out float number))
        {
            return new NumberToken(number);
        }

        _chars.MoveNext();
        throw new ParsingException($"Error: unexpected token at pos {_chars.Index + 1}: {_chars.Current}");
    }

    private bool ReadIdentifier(out string identifier)
    {
        identifier = "";
        while (true)
        {
            if (_chars.MoveNextIf(char.IsAsciiLetter))
            {
                identifier += _chars.Current;
            }
            else
            {
                break;
            }
        }
        return identifier.Length != 0;
    }

    private bool ReadNumber(out float number)
    {
        number = 0;

        string numStr = "";
        while (true)
        {
            if (_chars.MoveNextIf(c => char.IsNumber(c) || c == '.'))
            {
                numStr += _chars.Current;
            }
            else
            {
                break;
            }
        }
        if (numStr.Length != 0)
        {
            try
            {
                number = float.Parse(numStr);
                if (!float.IsFinite(number))
                {
                    throw new ParsingException($"Error: number `{numStr}` is too big");
                }
            }
            catch (Exception ex)
            {
                throw new ParsingException($"Error: number `{numStr}` is not in a valid format", ex);
            }
            return true;
        }

        return false;
    }
}
