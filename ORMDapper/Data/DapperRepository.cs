using DapperQueryBuilder;

namespace ORMDapper.Data;

public class DapperRepository : IDapperRepository
{
    private const string ConnectionString = "Data Source=EPGETBIW03B6;Initial Catalog=ORMFundamentals;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    private readonly IDbConnection _connection;

    public DapperRepository()
    {
        _connection = new DbConnection(ConnectionString);
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
        return _connection.Execute<T>(sql, param);
    }
}