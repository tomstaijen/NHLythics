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
                    RegisterProblem(ProblemType.ExtraTable, Severity.Low, entity);
                }
                else
                {
                    entity.Table = table;
                    if (entity.IsSynonym)
                        RegisterProblem(ProblemType.ExtraTable, Severity.Moderate, entity, "Synonym expected instead");
                }
            }

            // van alle synoniemen de target table zoeken
            // TODO: iets doen met entities met synoniem met lege target
            foreach (
                var entity in
                    Model.Entities.Where(e => e.Value.IsSynonym && string.IsNullOrEmpty(e.Value.Synonym.Target)))
            {
                RegisterProblem(ProblemType.IncompleteMapping, Severity.Important, entity.Value, "Synonym target not specified");
            }

            var entitiesByDatabase = Model.Entities.Where(e => e.Value.IsSynonym && !string.IsNullOrEmpty(e.Value.Synonym.Target)).Select(e => e.Value).GroupBy(e => e.Synonym.Database);

            foreach (var db in entitiesByDatabase)
            {
                var reader = new DatabaseReader(databaseReader.DatabaseSchema.ConnectionString.ChangeDatabase(db.Key), SqlType.SqlServer);
                var tables = reader.AllTables().ToDictionary(t => t.Name.ToLower(), t => t);
                
                foreach (var entity in db.ToList())
                {
                    var table = tables.GetValueOrDefault(entity.Synonym.Table.ToLower());
                    entity.Table = table;
                }
            }
        }
    }
}
