using System.Data.SqlClient;
using Dapper;
using DapperQueryBuilder;

namespace ORMDapper.Data;

public class DbConnection : IDbConnection
{
    private readonly SqlConnection _connection;

    public DbConnection(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
    }

    public IEnumerable<T> Query<T>(string sql)
    {
        return _connection.Query<T>(sql);
    }

    public IEnumerable<T> Query<T>(string sql, object param = null)
    {
        return _connection.Query<T>(sql, param);
    }

    public QueryBuilder QueryBuilder(FormattableString sql)
    {
        return _connection.QueryBuilder(sql);
    }

    public int Execute<T>(string sql, object param = null)
    {
        return _connection.Execute(sql, param);
    }
}