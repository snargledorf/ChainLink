using System;
using System.Linq;
using System.Reflection;

namespace ChainLink
{
    internal static class ReflectionUtils
    {
        public static T CreateObject<T>(object[] args)
        {
            if (args.Length == 0)
                return Activator.CreateInstance<T>();

            ConstructorInfo constructorInfo = typeof(T).GetConstructor(args.Select(arg => arg.GetType()).ToArray());
            return (T)constructorInfo.Invoke(args);
        }
    }
}
