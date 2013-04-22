using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace NHLythics.Test
{
    public class TestBase
    {
        private Configuration _configuration;

        protected Configuration Configuration
        {
            get
            {
                return _configuration;
            }
        }

        public string GetConnectionString(string datasource, string catalog, string user, string pass)
        {
            return string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", datasource, catalog, user, pass);
        }

        public void DefaultArrange()
        {
            _configuration = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008)
                    .Mappings(m => m.FluentMappings.AddFromAssembly(this.GetType().Assembly))
                    .BuildConfiguration();
        }
    }
}
