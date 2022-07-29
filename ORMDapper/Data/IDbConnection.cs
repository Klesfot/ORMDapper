using DapperQueryBuilder;

namespace ORMDapper.Data;

public interface IDbConnection
{
    IEnumerable<T> Query<T>(string sql);
    IEnumerable<T> Query<T>(string sql, object param = null);
    QueryBuilder QueryBuilder(FormattableString sql);
    int Execute<T>(string sql, object param = null);
}