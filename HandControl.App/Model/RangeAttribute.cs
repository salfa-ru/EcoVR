using System;

namespace HandControl.App.Model;

[AttributeUsage(AttributeTargets.Property)]
public class RangeIntAttribute : Attribute
{
    public int MinValue { get; }
    public int MaxValue { get; }

    public RangeIntAttribute(int minValue, int maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public bool IsValid(int value)
    {
        return value >= MinValue && value <= MaxValue;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class RangeDoubleAttribute : Attribute
{
    public double MinValue { get; }
    public double MaxValue { get; }

    public RangeDoubleAttribute(double minValue, double maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public bool IsValid(double value)
    {
        return value >= MinValue && value <= MaxValue;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class RangeFloatAttribute : Attribute
{
    public float MinValue { get; }
    public float MaxValue { get; }

    public RangeFloatAttribute(float minValue, float maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public bool IsValid(float value)
    {
        return value >= MinValue && value <= MaxValue;
    }
}