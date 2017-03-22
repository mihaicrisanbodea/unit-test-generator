using System;
using System.Collections.Generic;

namespace TestsGenerator.Services
{
    public static class ParameterGenerator
    {
        private static Dictionary<Type, Func<object>> dict;

        static ParameterGenerator()
        {
            BuildDictionary();
        }

        public static object GetValue(Type parameterType)
        {
            Func<object> func;
            if (dict.TryGetValue(parameterType, out func))
            {
                return func();
            }

            return null;
        }

        private static void BuildDictionary()
        {
            dict = new Dictionary<Type, Func<object>>
            {
                {typeof(int), GetInt},
                {typeof(bool), GetBool},
                {typeof(string), GetString},
                {typeof(float), GetFloat},
                {typeof(double), GetDouble},
                {typeof(DateTime), GetDateTime},
                {typeof(byte), GetByte},
                {typeof(long), GetLong},
                {typeof(decimal), GetDecimal},
                {typeof(sbyte), GetSbyte},
                {typeof(short), GetShort},
                {typeof(uint), GetUint},
                {typeof(ulong), GetUlong},
                {typeof(ushort), GetUshort}
            };
        }

        private static object GetString()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);
        }

        private static object GetBool()
        {
            var random = new Random();
            return random.Next(100) <= 20;
        }

        private static object GetInt()
        {
            var random = new Random();
            return random.Next(100);
        }

        private static object GetFloat()
        {
            var random = new Random();
            return float.MaxValue * (random.Next() / 1073741824.0f - 1.0f);
        }

        private static object GetDouble()
        {
            var random = new Random();
            return double.MaxValue * (random.Next() / 1073741824.0d - 1.0d);
        }

        private static object GetDateTime()
        {
            return DateTime.UtcNow;
        }

        private static object GetByte()
        {
            var random = new Random();
            return random.Next(255);
        }

        private static object GetLong()
        {
            var random = new Random();
            return long.MaxValue * (random.Next() / 1073741824.0f - 1.0f);
        }

        private static object GetSbyte()
        {
            var random = new Random();
            return random.Next(127);
        }

        private static object GetShort()
        {
            var random = new Random();
            return random.Next(32767);
        }

        private static object GetUint()
        {
            var random = new Random();
            return random.Next(42949672);
        }

        private static object GetUlong()
        {
            var random = new Random();
            return random.Next(42949672);
        }

        private static object GetUshort()
        {
            var random = new Random();
            return random.Next(65535);
        }

        private static object GetDecimal()
        {
            var random = new Random();
            return random.Next(1000);
        }
    }
}
