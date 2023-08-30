namespace computorv1.Collections;

using System.Collections;

public class LexerEnumerator : IEnumerator<char>
{
    private string _str;
    private int _index;
    private char _currentElement;

    public LexerEnumerator(string str)
    {
        _str = str;
        Reset();
    }

    public int Index => _index;

    public char Current => _currentElement;

    object IEnumerator.Current => Current;

    public char Peek() => _index < _str.Length - 1 ? _str[_index + 1] : default;

    public bool MoveNextIf(Func<char, bool> predicate)
    {
        if (_index < _str.Length - 1)
        {
            if (predicate.Invoke(_str[_index + 1]))
            {
                _index++;
                _currentElement = _str[_index];
                return true;
            }
            return false;
        }
        _index = _str.Length;
        return false;
    }

    public bool MoveNext()
    {
        if (_index < _str.Length - 1)
        {
            _index++;
            _currentElement = _str[_index];
            return true;
        }
        _index = _str.Length;
        return false;
    }

    public void Dispose()
    {
        if (_str != null)
        {
            _index = _str.Length;
        }
        _str = null!;
    }

    public void Reset()
    {
        _currentElement = default;
        _index = -1;
    }
}
