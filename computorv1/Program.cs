using System.Globalization;
using computorv1;

if (args.Length != 1)
{
	Console.WriteLine("Usage: dotnet run <equation>");
	return 1;
}

string line = args[0];
List<Token> tokens = Tokenize(new LexerEnumerator(line));
foreach (Token token in tokens)
{
	Console.WriteLine(token);
}

return 0;

List<Token> Tokenize(LexerEnumerator chars)
{
	List<Token> tokens = new();
    while (NextToken(chars, out Token token))
    {
        tokens.Add(token);
	}

	return tokens;
}

bool NextToken(LexerEnumerator chars, out Token token)
{
    while (chars.MoveNextIf(char.IsWhiteSpace))
	{
	}
	char c = chars.Peek();
    token = c switch
	{
		'+' => new(), // Token.Add(c),
		'-' => new(), // Token.Sub(c),
		'*' => new(), // Token.Mul(c),
        '^' => new(), // Token.Pow(c),
		'=' => new(), // Token.Equal(c),
        '\0' => new(), // Token.EOF,
		_ => NextComplexToken(chars),
	};
	chars.MoveNext();
    return c != '\0';
}

Token NextComplexToken(LexerEnumerator chars)
{
    if (ReadIdentifier(chars, out string identifier))
    {
		return new(); // Token.Identifier(identifier);
    }
    if (ReadNumber(chars, out float number))
    {
        return new(); // Token.Number(number);
    }
	throw new Exception($"Error parsing token at pos {chars.Index + 1} ({chars.Peek()}).");
}

bool ReadIdentifier(LexerEnumerator chars, out string identifier)
{
    identifier = "";
	while (true)
	{
		if (chars.MoveNextIf(char.IsAsciiLetter))
		{
            identifier += chars.Current;
		}
		else
		{
			break;
		}
	}
	return identifier.Length != 0;
}

bool ReadNumber(LexerEnumerator chars, out float number)
{
	string numStr = "";
	while (true)
	{
		if (chars.MoveNextIf(c => char.IsNumber(c) || c == '.'))
		{
			numStr += chars.Current;
		}
		else
		{
			break;
		}
	}
	if (numStr.Length != 0)
	{
		number = float.Parse(numStr, CultureInfo.InvariantCulture);
		return true;
	}

	number = 0;
	return false;
}

class Token { }
