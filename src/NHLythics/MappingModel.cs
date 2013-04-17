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

        public void AnalyseClasses()
        {
            Classes.ForEach(Validate);
        }

        public void Validate(PersistentClass pc)
        {
            var table = Tables.Where(t => t.Name == pc.Table.Name).SingleOrDefault();

            foreach (var property in pc.PropertyIterator)
            {
                foreach (var column in property.ColumnIterator.OfType<Column>())
                {
                    var dbColumn = (table == null ? null : table.Columns.SingleOrDefault(c => c.Name == column.Text));
                    Validate(property, column, dbColumn);
                }
            }
        }

        public void Validate(Property property, Column mColumn, DatabaseColumn dbColumn)
        {
            Console.WriteLine("Hallo");
        }
    }
}
