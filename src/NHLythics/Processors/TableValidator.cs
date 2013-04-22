using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseSchemaReader.DataSchema;
using NHLythics.Extensions;
using NHLythics.Model;
using Attribute = NHLythics.Model.Attribute;

namespace NHLythics
{
    class TableValidator : IEntityProcessor
    {
        public IEnumerable<Problem> Process(Entity entity)
        {
            if (entity.Table == null)
                yield return new Problem {Location = entity, Description = "Table not found" };

            var pair = entity.Table.Columns.Pair(entity.Attributes, (c, a) => c.Name == a.Column.Name);
            
            foreach (var noS in pair.NoFirst)
            {
                yield return new Problem() {Location = noS, Description = "Missing column", Solution = "Add column"};
            }
            foreach (var noF in pair.NoSecond)
            {
                if( !noF.Nullable)
                    yield return new Problem { Location = entity, Description = "Extra column NOT NULLABLE", Solution = "Remove column"};
                else
                    yield return new Problem { Location = entity, Description = "Extra column", Solution = "Remove column" };
            }
            foreach (var p in pair.Matches)
            {
                var problem = ValidateColumn(p.Item2, p.Item1);
                if (problem != null)
                    yield return problem;
            }
        }

        public Problem ValidateColumn(Attribute attribute, DatabaseColumn column)
        {
            return null;
        }
    }

    public static class TableValidatorModelExtensions
    {
        public static void ValidateDatabase(this MappingModel model)
        {
            model.Apply(new TableValidator());
        }
    }
}
