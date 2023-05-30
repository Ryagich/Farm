using System;

static class EnumExtentions
{
    public static T Cycle<T>(T value) where T : struct, IConvertible
    {
        var type = typeof(T);
        if (!type.IsEnum)
            throw new ArgumentException("T must be an enumerated type");
        var values = (T[])Enum.GetValues(type);
        for (int i = 0; i < values.Length; i++)
            if (values[i].Equals(value))
                return values[(i + 1) % values.Length];
        throw new ArgumentException(nameof(value));
    }
}
