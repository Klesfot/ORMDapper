using System.Data.SqlClient;
using Dapper;

namespace ORMDapper.Data;

public class DapperRepository : IDapperRepository
{
    private const string ConnectionString = "Data Source=EPGETBIW03B6;Initial Catalog=ORMFundamentals;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    public IEnumerable<T> Query<T>(string sql)
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            return conn.Query<T>(sql);
        }
    }

    public int Execute<T>(string sql, object param = null)
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            return conn.Execute(sql, param);
        }
    }
}