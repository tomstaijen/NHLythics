using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;

namespace NHLythics.Extensions
{
    public static class ConnectionStringExtionsions
    {
        public static string ChangeDatabase(this string connectionString, string database, string schema = null)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = database;
            return builder.ConnectionString;
        }
    }
}
