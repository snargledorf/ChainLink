using System;
using System.Linq;
using System.Reflection;

namespace ChainLink
{
    internal static class ReflectionUtils
    {
        public static object CreateObject(Type type, object[] args)
        {
            if (args.Length == 0)
                return Activator.CreateInstance(type);

            ConstructorInfo constructorInfo = type.GetConstructor(args.Select(arg => arg.GetType()).ToArray());
            return constructorInfo.Invoke(args);
        }
    }
}
