using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseSchemaReader.DataSchema;
using NHLythics.Extensions;
using NHLythics.Model;
using NHibernate.Dialect;
using NHibernate.Engine;
using Attribute = NHLythics.Model.Attribute;

namespace NHLythics
{
    class TableValidator : EntityCheckerBase
    {
        private Dialect _dialect;
        private IMapping _mapping;
        public TableValidator(Dialect dialect, IMapping mapping)
        {
            _dialect = dialect;
            _mapping = mapping;
        }

        public override void Check(Entity entity)
        {
            if (!entity.IsTable) // this is an "extra" table, so skipping
                return;

            if (entity.Table == null)
            {
                RegisterProblem(ProblemType.MissingTable, Severity.Critical, entity);
                return;
            }

            var pair = entity.Table.Columns.Pair(entity.Attributes, (c, a) => c.Name.ToLower() == a.Column.Name.ToLower());
            
            foreach (var noS in pair.NoFirst)
            {
                RegisterProblem(ProblemType.MissingColumn, Severity.Critical, noS);
            }
            foreach (var noF in pair.NoSecond)
            {
                if( !noF.Nullable)
                    RegisterProblem(ProblemType.ExtraColumnNotNullable, (entity.IsReadOnly) ? Severity.Low : Severity.Critical, entity, noF.Name);
                else
                    RegisterProblem(ProblemType.ExtraColumn, Severity.Low, entity, noF.Name);
            }
            foreach (var p in pair.Matches)
            {
                ValidateColumn(p.Item2, p.Item1);
            }
        }

        public string BaseSqlType(string basetype)
        {
            var type = basetype;
            if (type.Contains("("))
                type = type.Substring(0, type.IndexOf("("));
            return type.ToLower();
        }

        public void ValidateColumn(Attribute attribute, DatabaseColumn column)
        {
            var mappingType = attribute.Column.GetSqlType(_dialect, _mapping);
            var baseType = BaseSqlType(mappingType);

            if (baseType != column.DbDataType)
                RegisterProblem(ProblemType.ColumnTypeMismatch, Severity.Important, attribute,
                                string.Format("mapping={0},db={1}", baseType, column.DbDataType));
            else
            {
                if (baseType == "decimal")
                {
                    var mappingDecimal = string.Format("({0},{1})", attribute.Column.Precision, attribute.Column.Scale);
                    var dbDecimal = string.Format("({0},{1}", column.Precision, column.Scale);

                    if (mappingDecimal == dbDecimal)
                        RegisterProblem(ProblemType.ColumnTypeMismatch, Severity.Important, attribute, string.Format("decimal size mismatch: mapping={0},db={1}", mappingDecimal, dbDecimal));
                }
                else if (baseType.Contains("char")) // compare length
                {
                    if (attribute.Column.Length != column.Length)
                    {
                        RegisterProblem(ProblemType.ColumnTypeMismatch, Severity.Moderate, attribute, string.Format("String length mismatch: mapping={0},db={1}", mappingType.Length, column.Length));
                    }
                }
            }

            if (attribute.IsOptional && !column.Nullable)
            {
                RegisterProblem(ProblemType.ColumnTypeMismatch, Severity.Critical, attribute, "Column should be NULLABLE but it is NOT");   
            }
            else if (!attribute.IsOptional && column.Nullable)
            {
                RegisterProblem(ProblemType.ColumnTypeMismatch, Severity.Moderate, attribute, "Column can be made NOT NULLABLE which it is not.");   
            }
        }
    }
}
