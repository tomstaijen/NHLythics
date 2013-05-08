using System;
using System.Collections.Generic;
using NHLythics.Model;

namespace NHLythics
{
    public static class ModelRegistry
    {
        private static IDictionary<Type, Synonym> typeSynonyms = new Dictionary<Type, Synonym>();
        private static IDictionary<string, Synonym> tableSynonyms = new Dictionary<string, Synonym>();

        private static IDictionary<Type, string> views = new Dictionary<Type, string>();
        private static IList<Type> removables = new List<Type>();
        private static IDictionary<Type, Tuple<string, string>> sps = new Dictionary<Type, Tuple<string, string>>();


        public static void RegisterSynonym(Type t, Synonym synonym)
        {
            if(!typeSynonyms.ContainsKey(t))
                typeSynonyms.Add(t, synonym);
        }

        public static void RegisterSynonym(string table, Synonym synonym)
        {
            if (!tableSynonyms.ContainsKey(table))
                tableSynonyms.Add(table, synonym);
        }

        public static void RegisterView(Type t, string query)
        {
            if( !views.ContainsKey(t))
                views.Add(t, query);
        }

        public static void RegisterRemovable(Type t)
        {
            if( !removables.Contains(t))
                removables.Add(t);
        }

        public static void RegisterSP(Type t, string name, string query)
        {
            if( !sps.ContainsKey(t))
                sps.Add(t, new Tuple<string, string>(name, query));
        }

        public static bool IsView(Type t)
        {
            return views.ContainsKey(t);
        }

        public static bool IsSP(Type t)
        {
            return sps.ContainsKey(t);
        }

        public static bool IsSynonym(Type t)
        {
            return GetSynonym(t) != null;
        }

        public static bool IsSynonym(string table)
        {
            return GetSynonym(table) != null;
        }

        public static Synonym GetSynonym(Type t)
        {
            if (!typeSynonyms.ContainsKey(t))
                return null;
            return typeSynonyms[t];
        }

        public static Synonym GetSynonym(string table)
        {
            if (!tableSynonyms.ContainsKey(table))
                return null;
            return tableSynonyms[table];
        }


    }
}