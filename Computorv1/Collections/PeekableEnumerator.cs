namespace Computorv1.Collections;

using System.Collections;

public class PeekableEnumerator<T> : IEnumerator<T>
{
    private IReadOnlyList<T> _src;
    private int _index;
    private T? _currentElement;

    public PeekableEnumerator(IReadOnlyList<T> src)
    {
        _src = src;
        Reset();
    }

    public int Index => _index;

    public T Current => _currentElement!;

    object IEnumerator.Current => Current!;

    public T? Peek() => _index < _src.Count - 1 ? _src[_index + 1] : default;

    public bool MoveNextIf(Func<T, bool> predicate)
    {
        if (_index < _src.Count - 1)
        {
            if (predicate.Invoke(_src[_index + 1]))
            {
                _index++;
                _currentElement = _src[_index];
                return true;
            }
            return false;
        }
        _index = _src.Count;
        return false;
    }

    public bool MoveNext()
    {
        if (_index < _src.Count - 1)
        {
            _index++;
            _currentElement = _src[_index];
            return true;
        }
        _index = _src.Count;
        _currentElement = default;
        return false;
    }

    public void Dispose()
    {
        if (_src != null)
        {
            _index = _src.Count;
        }
        _src = null!;

        GC.SuppressFinalize(this);
    }

    public void Reset()
    {
        _currentElement = default;
        _index = -1;
    }
}
