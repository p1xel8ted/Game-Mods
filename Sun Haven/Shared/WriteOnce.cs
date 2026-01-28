using System;

namespace Shared;

public sealed class WriteOnce<T>
{
    private T _value;
    public WriteOnce(T value)
    {
        Value = value;
    }

    public WriteOnce()
    {
    }

    public override string ToString()
    {
        return HasValue ? Convert.ToString(_value) : "";
    }

    public T Value
    {
        get => !HasValue ? default : _value;
        set
        {
            if (HasValue) return;
            _value = value;
            HasValue = true;
        }
    }

    public bool HasValue { get; private set; }

    public static implicit operator T(WriteOnce<T> value)
    {
        return value.Value;
    }
}