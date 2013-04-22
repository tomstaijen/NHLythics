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
    class TableValidator : EntityCheckerBase
    {
        public override void Check(Entity entity)
        {
            if (entity.Table == null)
            {
                RegisterProblem(new Problem {Location = entity, Description = "Table not found"});
                return;
            }


            var pair = entity.Table.Columns.Pair(entity.Attributes, (c, a) => c.Name == a.Column.Name);
            
            foreach (var noS in pair.NoFirst)
            {
                RegisterProblem(new Problem() {Location = noS, Description = "Missing column", Solution = "Add column"});
            }
            foreach (var noF in pair.NoSecond)
            {
                if( !noF.Nullable)
                    RegisterProblem(new Problem { Location = entity, Description = "Extra column NOT NULLABLE", Solution = "Remove column"});
                else
                    RegisterProblem(new Problem { Location = entity, Description = "Extra column", Solution = "Remove column" });
            }
            foreach (var p in pair.Matches)
            {
                ValidateColumn(p.Item2, p.Item1);
            }
        }

        public void ValidateColumn(Attribute attribute, DatabaseColumn column)
        {
        }
    }

    public static class TableValidatorCheckerExtensions
    {
        public static void ValidateDatabase(this ModelChecker checker)
        {
            checker.Apply(new TableValidator());
        }
    }
}
