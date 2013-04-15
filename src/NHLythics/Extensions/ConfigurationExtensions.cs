using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NHLythics.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Func<object> GetConnection(this Configuration configuration)
        {
            ManagedProviderConnectionHelper helper = new ManagedProviderConnectionHelper(configuration.Properties);
            
            using (helper.Connection)
            {
                
            }
            return null;
        }
    }
}
