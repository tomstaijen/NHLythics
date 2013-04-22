using System;
using System.Collections.Generic;
using NHLythics.Model;

namespace NHLythics
{
    public static class ModelRegistry
    {
        private static IDictionary<Type, Synonym> synonyms = new Dictionary<Type, Synonym>();

        public static void RegisterSynonym(Type t, Synonym synonym)
        {
            if(!synonyms.ContainsKey(t))
                synonyms.Add(t, synonym);
        }

        public static bool IsSynonym(Type t)
        {
            return GetSynonym(t) != null;
        }

        public static Synonym GetSynonym(Type t)
        {
            if (!synonyms.ContainsKey(t))
                return null;
            return synonyms[t];
        }

    }
}