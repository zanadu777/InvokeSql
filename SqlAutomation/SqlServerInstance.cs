using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace SqlAutomation
{
    public class SqlServerInstance
    {
        public SqlServerInstance(string connectionString)
        {
            var sqlconnection = new SqlConnection(connectionString);
            var connection = new ServerConnection(sqlconnection);
            server = new Server(connection);
        }

        private Server server;

        public List<DataSet> ExecuteQuery(IEnumerable<string> sqls)
        {
            var results = new List<DataSet>();
            foreach (var sql in sqls)
                results.Add(server.ConnectionContext.ExecuteWithResults(sql));

            return results;
        }

        public DataSet ExecuteQuery(string sql)
        {
            var result = server.ConnectionContext.ExecuteWithResults(sql);

            return result;
        }


        public void ExecuteNonQuery(IEnumerable<string> sqls)
        {
            foreach (var sql in sqls)
                server.ConnectionContext.ExecuteNonQuery(sql);
        }

        public List<object> ExecuteScalar(IEnumerable<string> sqls)
        {
            var results = new List<object>();
            foreach (var sql in sqls)
                results.Add(server.ConnectionContext.ExecuteScalar(sql));

            return results;
        }
    }
}
