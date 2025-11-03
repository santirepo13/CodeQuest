using System;

namespace CodeQuest.Utils
{
    public static class SafeConverter
    {
        public static int ToInt32(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;
                
            try
            {
                // Handle common SQL Server return types
                switch (value)
                {
                    case int intValue:
                        return intValue;
                    case long longValue:
                        return (int)longValue;
                    case decimal decimalValue:
                        return (int)decimalValue;
                    case double doubleValue:
                        return (int)doubleValue;
                    case float floatValue:
                        return (int)floatValue;
                    case byte byteValue:
                        return (int)byteValue;
                    case short shortValue:
                        return (int)shortValue;
                    case string stringValue:
                        return int.Parse(stringValue);
                    default:
                        return Convert.ToInt32(value);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Cannot convert {value.GetType().Name} value '{value}' to Int32: {ex.Message}");
            }
        }
    }
}