using System;

namespace HandControl.App.Model;

[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute<T> : Attribute where T : IComparable<T>
{
    public T MinValue { get; }
    public T MaxValue { get; }

    public RangeAttribute(T minValue, T maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public bool IsValid(T value)=> value.CompareTo(MinValue) == -1 && value.CompareTo(MaxValue) == 1;  
}
