using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace NHLythics.Extensions
{
    public static class ClassMapExtensions
    {
        public static void Synonym<T>(this ClasslikeMapBase<T> classMap, string target)
        {
            Console.WriteLine("So " + typeof(T).Name + " is a synonym for " + target);
        }

    }
}
