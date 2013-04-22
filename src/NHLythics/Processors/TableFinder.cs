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
    public class TableFinder : ModelCheckerModuleBase
    {
        public string ConnectionString { get; set; }

        public override void Run()
        {
            var databaseReader = new DatabaseReader(ConnectionString, SqlType.SqlServer);

            foreach (var table in databaseReader.AllTables())
            {
                var entity = Model.GetEntityByName(table.Name);
                if (entity == null)
                {
                    entity = new Entity(Model) { Name = table.Name, Table = table };
                    Model.AddEntity(entity);
                    RegisterProblem(new UnknownTableProblem{ Location = entity, Solution = "DROP TABLE", Description = "Unknown table" });
                }
                else
                {
                    entity.Table = table;
                    if (entity.IsSynonym)
                        RegisterProblem(new Problem {Location = entity, Solution = "REMOVE SYNONYM", Description = "Table exist for synonym"});
                }
            }

            // van alle synoniemen de target table zoeken
            var entitiesByDatabase = Model.Entities.Where(e => e.Value.IsSynonym).Select(e => e.Value).GroupBy(e => e.Synonym.Database);

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
