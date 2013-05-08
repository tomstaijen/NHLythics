using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHLythics.Model;

namespace NHLythics.Mapping
{
    public class NClassMap<T> : ClassMap<T>
    {
        public void Synonym(string target)
        {
            ModelRegistry.RegisterSynonym(typeof(T), new Synonym { Target = target });
        }

        public void View(string viewQuery)
        {
            ModelRegistry.RegisterView(typeof(T), viewQuery);
        }

        public void SP(string name, string query)
        {
            ModelRegistry.RegisterSP(typeof(T), name, query);
        }

        public void Removable()
        {
            ModelRegistry.RegisterRemovable(typeof(T));
        }
    }

    public static class ManyToManyPartExtensions
    {
        public static ManyToManyPart<T> Synonym<T>(this ManyToManyPart<T> mtm, string synonym, string target)
        {
            ModelRegistry.RegisterSynonym(synonym, new Synonym { Target = target });
            return mtm;
        }
    }
}
