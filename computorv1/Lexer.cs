using System.Globalization;
using computorv1.Tokens;

namespace computorv1
{
    public struct Lexer
    {
        private readonly LexerEnumerator _chars;

        public Lexer(string str)
        {
            _chars = new LexerEnumerator(str);
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while reading input: {ex.Message}");
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
            _chars.MoveNext();
            return c != '\0';
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
            throw new Exception($"Error parsing token at pos {_chars.Index + 1} ({_chars.Peek()}).");
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
                number = float.Parse(numStr);
                return true;
            }

            number = 0;
            return false;
        }
    }
}
