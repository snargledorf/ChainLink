using System;

namespace ChainLink
{
    public class ChainLinkDescription
    {
        public ChainLinkDescription(Type type, object[] args)
        {
            Type = type;
            Args = args;
        }

        public Type Type { get; }

        public object[] Args { get; }
    }
}