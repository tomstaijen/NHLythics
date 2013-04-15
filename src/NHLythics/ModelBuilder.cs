using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using NHibernate.Cfg;

namespace NHLythics
{
    public class ModelBuilder
    {
        public ModelBuilder(Configuration configuration)
        {
            var mappings = configuration.BuildMapping();
            var s = mappings.ToString();
            configuration.BuildMappings();
            var classMappings = configuration.ClassMappings;


            var schemaReader = new DatabaseReader(null, SqlType.SqlServer);
        }
    }
}
