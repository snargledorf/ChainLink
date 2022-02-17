using System.Collections.Generic;

namespace ChainLink
{
    internal class ChainLinkRunContext : IChainLinkRunContext
    {
        private readonly Dictionary<string, object> state = new Dictionary<string, object>();

        public object this[string variableName]
        {
            get => state[variableName];
            set => state[variableName] = value;
        }

        public bool Has(string varibaleName) => state.ContainsKey(varibaleName);

        public T Get<T>(string variableName)
        {
            return (T)this[variableName];
        }
    }
}