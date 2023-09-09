namespace Computorv1.Collections;

using System.Collections;
using Computorv1.Tokens;

public class TokenEnumerator : IEnumerator<Token>
{
    private IReadOnlyList<Token> _tokens;
    private int _index;
    private Token? _currentElement;

    public TokenEnumerator(IReadOnlyList<Token> tokens)
    {
        _tokens = tokens;
        Reset();
    }

    public Token Current => _currentElement!;

    object IEnumerator.Current => Current;

    public Token? Peek() => _index < _tokens.Count - 1 ? _tokens[_index + 1] : default;

    public bool MoveNext()
    {
        if (_index < _tokens.Count - 1)
        {
            _index++;
            _currentElement = _tokens[_index];
            return true;
        }
        _index = _tokens.Count;
        return false;
    }

    public void Dispose()
    {
        if (_tokens != null)
        {
            _index = _tokens.Count;
        }
        _tokens = null!;

        GC.SuppressFinalize(this);
    }

    public void Reset()
    {
        _currentElement = default;
        _index = -1;
    }
}
