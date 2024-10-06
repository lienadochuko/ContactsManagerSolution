using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Infastructure.Repositories.DataAccess
{
	public interface IDataRepository
	{
		Task<IEnumerable<T>> GetListOfActors<T>(string procedureName, string ConnectionString, SqlParameter[] sqlParameter, Func<SqlDataReader, T> mapFunction, CancellationToken cancellationToken = default);
	}

    public class DataRepository : IDataRepository
    {
        public async Task<IEnumerable<T>> GetListOfActors<T>(string procedureName, string connectionString, SqlParameter[] sqlParameter, Func<SqlDataReader, T> mapFunction, CancellationToken cancellationToken = default)
        {
            await using SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new()
            {
                CommandText = procedureName,
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 300
            };

            if (sqlParameter != null)
            {
                command.Parameters.AddRange(sqlParameter);
            }

            await connection.OpenAsync(cancellationToken);

            List<T> result = new List<T>();

            await using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    T record = mapFunction(reader);
                    result.Add(record);
                }
            }

            return result;
        }
    }
}
