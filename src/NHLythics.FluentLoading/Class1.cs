using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace NHLythics.FluentLoading
{
    public static class ModelBuilderExtensions
    {

        public static void WithMappingsFromAssembliesIn(this ModelBuilder builder, Assembly[] assemblies)
        {

            var config = Fluently.Configure()
                                 .Database(MsSqlConfiguration.MsSql2008)
                                 .Mappings(m =>
                                     {
                                         foreach (var a in assemblies)
                                         {
                                             m.FluentMappings.AddFromAssembly(a);
                                         }
                                     })
                                 .BuildConfiguration();
            builder.ApplyMappings(config);
        }
    }
}
