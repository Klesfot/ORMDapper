namespace ORMDapper.Data;

public interface IDapperRepository
{
    IEnumerable<T> Query<T>(string sql);
    int Execute<T>(string sql, object param = null);
}