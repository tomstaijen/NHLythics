using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHLythics.Model;

namespace NHLythics.Extensions
{
    public static class ClassMapExtensions
    {
        public static void Synonym<T>(this ClasslikeMapBase<T> classMap, string target)
        {
            ModelRegistry.RegisterSynonym(typeof(T), new Synonym { Target = target });
        }

    }
}
