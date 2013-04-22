using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using NHLythics.Model;
using NHLythics.Processors;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHLythics
{
    public class ModelBuilder
    {
        private MappingModel _model;
        
        public ModelBuilder(MappingModel model)
        {
            _model = model;
        }

        public void ApplyMappings(Configuration configuration)
        {
            Apply(new EntityHarvester { Configuration = configuration });
            _model.Apply(new AttributeHarvester(_model));
        }

        public void Apply(IModelBuilderExtension builder)
        {
            _model.Problems.AddRange(builder.Build(_model));
        }

        public void ApplyDatabase(string connectionString)
        {
            Apply(new TableFinder { ConnectionString = connectionString });
        }
    }
}
