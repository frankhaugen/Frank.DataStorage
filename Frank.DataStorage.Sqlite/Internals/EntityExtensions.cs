using System.Diagnostics;
using System.Reflection;

using Frank.DataStorage.Abstractions;

namespace Frank.DataStorage.Sqlite;

internal static class EntityExtensions
{
    
    // ReSharper disable once CognitiveComplexity
    public static void SetPropertyValue<T>(this T entity, Type fieldType, PropertyInfo property, object value) where T : class, IKeyed, new()
    {
        if (fieldType == typeof(Guid))
        {
            property.SetValue(entity, Guid.Parse(value.ToString() ?? string.Empty));
        }
        else if (fieldType == typeof(Guid?))
        {
            property.SetValue(entity, Guid.TryParse(value.ToString(), out var guid)
                ? guid
                : null);
        }
        else if (fieldType == typeof(int))
        {
            property.SetValue(entity, Convert.ToInt32(value));
        }
        else if (fieldType == typeof(int?))
        {
            property.SetValue(entity, Convert.ToInt32(value));
        }
        else if (fieldType == typeof(long))
        {
            property.SetValue(entity, Convert.ToInt64(value));
        }
        else if (fieldType == typeof(long?))
        {
            property.SetValue(entity, Convert.ToInt64(value));
        }
        else if (fieldType == typeof(short))
        {
            property.SetValue(entity, Convert.ToInt16(value));
        }
        else if (fieldType == typeof(short?))
        {
            property.SetValue(entity, Convert.ToInt16(value));
        }
        else if (fieldType == typeof(byte))
        {
            property.SetValue(entity, Convert.ToByte(value));
        }
        else if (fieldType == typeof(byte?))
        {
            property.SetValue(entity, Convert.ToByte(value));
        }
        else if (fieldType == typeof(uint))
        {
            property.SetValue(entity, Convert.ToUInt32(value));
        }
        else if (fieldType == typeof(uint?))
        {
            property.SetValue(entity, Convert.ToUInt32(value));
        }
        else if (fieldType == typeof(ulong))
        {
            property.SetValue(entity, Convert.ToUInt64(value));
        }
        else if (fieldType == typeof(ulong?))
        {
            property.SetValue(entity, Convert.ToUInt64(value));
        }
        else if (fieldType == typeof(ushort))
        {
            property.SetValue(entity, Convert.ToUInt16(value));
        }
        else if (fieldType == typeof(ushort?))
        {
            property.SetValue(entity, Convert.ToUInt16(value));
        }
        else if (fieldType == typeof(sbyte))
        {
            property.SetValue(entity, Convert.ToSByte(value));
        }
        else if (fieldType == typeof(sbyte?))
        {
            property.SetValue(entity, Convert.ToSByte(value));
        }
        else if (fieldType == typeof(char))
        {
            property.SetValue(entity, Convert.ToChar(value));
        }
        else if (fieldType == typeof(char?))
        {
            property.SetValue(entity, Convert.ToChar(value));
        }
        else if (fieldType == typeof(string))
        {
            property.SetValue(entity, Convert.ToString(value));
        }
        else if (fieldType == typeof(DateTime))
        {
            property.SetValue(entity, Convert.ToDateTime(value));
        }
        else if (fieldType == typeof(DateTime?))
        {
            property.SetValue(entity, Convert.ToDateTime(value));
        }
        else if (fieldType == typeof(bool))
        {
            property.SetValue(entity, Convert.ToBoolean(value));
        }
        else if (fieldType == typeof(bool?))
        {
            property.SetValue(entity, Convert.ToBoolean(value));
        }
        else if (fieldType == typeof(decimal))
        {
            property.SetValue(entity, Convert.ToDecimal(value));
        }
        else if (fieldType == typeof(decimal?))
        {
            property.SetValue(entity, Convert.ToDecimal(value));
        }
        else if (fieldType == typeof(TimeSpan))
        {
            property.SetValue(entity, TimeSpan.Parse(value.ToString() ?? string.Empty));
        }
        else if (fieldType == typeof(TimeSpan?))
        {
            property.SetValue(entity, TimeSpan.TryParse(value.ToString(), out var timeSpan)
                ? timeSpan
                : null);
        }
        else if (fieldType == typeof(DateTimeOffset))
        {
            property.SetValue(entity, DateTimeOffset.Parse(value.ToString() ?? string.Empty));
        }
        else if (fieldType == typeof(DateTimeOffset?))
        {
            property.SetValue(entity, DateTimeOffset.TryParse(value.ToString(), out var dateTimeOffset)
                ? dateTimeOffset
                : null);
        }
        else if (fieldType == typeof(byte[]))
        {
            property.SetValue(entity, (byte[])value);
        }
        else
        {
            Debugger.Break();
        }
    }
}