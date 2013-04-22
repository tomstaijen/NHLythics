using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using NHLythics.Extensions;
using NHLythics.Model;

namespace NHLythics
{
    public class TableFinder : IModelBuilderExtension
    {
        public string ConnectionString { get; set; }

        public IEnumerable<Problem> Build(MappingModel model)
        {
            var databaseReader = new DatabaseReader(ConnectionString, SqlType.SqlServer);

            foreach (var table in databaseReader.AllTables())
            {
                var entity = model.GetEntityByName(table.Name);
                if (entity == null)
                {
                    entity = new Entity(model) {Name = table.Name, Table = table};
                    model.AddEntity(entity.Name, entity);
                    yield return new UnknownTableProblem{ Location = entity, Solution = "DROP TABLE", Description = "Unknown table" };
                }
                else
                {
                    entity.Table = table;
                    if (entity.IsSynonym)
                        yield return new Problem {Location = entity, Solution = "REMOVE SYNONYM", Description = "Table exist for synonym"};
                }
            }

            // van alle synoniemen de target table zoeken
            var entitiesByDatabase = model.Entities.Where(e => e.Value.IsSynonym).Select(e => e.Value).GroupBy(e => e.Synonym.Database);

            foreach (var db in entitiesByDatabase)
            {
                var reader = new DatabaseReader(databaseReader.DatabaseSchema.ConnectionString.ChangeDatabase(db.Key), SqlType.SqlServer);
                var tables = reader.AllTables().ToDictionary(t => t.Name, t => t);
                
                foreach (var entity in db.ToList())
                {
                    var table = tables.GetValueOrDefault(entity.Synonym.Table);
                    entity.Table = table;
                }
            }
        }
    }
}
