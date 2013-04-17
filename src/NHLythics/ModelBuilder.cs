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
        private Configuration _configuration;
        private string _connectionString;

        public ModelBuilder(Configuration configuration, string connectionString = null)
        {
            _configuration = configuration;
            _connectionString = connectionString;
            _configuration.BuildMappings();
        }

        public MappingModel Build()
        {
            List<DatabaseTable> tables = new List<DatabaseTable>();
            if (!string.IsNullOrEmpty(_connectionString))
            {
                var schemaReader = new DatabaseReader(_connectionString, SqlType.SqlServer);
                tables = schemaReader.AllTables().ToList();
            }
            

            var mappingTables =
                _configuration.ClassMappings.Select(cm => cm.Table)
                              .Union(_configuration.CollectionMappings.Select(cm => cm.CollectionTable))
                              .Where(t => t != null)
                              .Distinct().ToList();

            return new MappingModel
                {
                    Classes = _configuration.ClassMappings.ToList(),
                    Collections = _configuration.CollectionMappings.ToList(),
                    Tables = tables,
                    MappingTables = mappingTables
                };
        }
    }
}
