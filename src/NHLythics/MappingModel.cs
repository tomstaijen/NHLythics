using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseSchemaReader.DataSchema;
using NHibernate.Mapping;

namespace NHLythics
{
    public class MappingModel
    {
        public List<DatabaseTable> Tables { get; set; }
        public List<Table> MappingTables { get; set; }
        public List<PersistentClass> Classes { get; set; }
        public List<Collection> Collections { get; set; }
    }
}
