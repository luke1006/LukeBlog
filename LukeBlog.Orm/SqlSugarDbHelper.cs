using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeBlog.Orm
{
   public  class SqlSugarDbHelper
    {
        public static string ConnString = ConfigurationManager.AppSettings["SqliteConnectionString"];

        public static SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            DbType = DbType.Sqlite,
            ConnectionString = ConnString,
            IsAutoCloseConnection = true
        });
    }
}
