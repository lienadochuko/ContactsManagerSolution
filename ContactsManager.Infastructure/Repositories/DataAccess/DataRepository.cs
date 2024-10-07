using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsManager.Core.DTO;

namespace ContactsManager.Infastructure.Repositories.DataAccess
{
	public interface IDataRepository
	{
		Task<IEnumerable<T>> GetListOfActors<T>(string procedureName, string ConnectionString, SqlParameter[] sqlParameter, Func<SqlDataReader, T> mapFunction, CancellationToken cancellationToken = default);
        Task<ActorsDto> GetSearchedActors(string id, string procedureName, string connectionString,CancellationToken cancellationToken = default);


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
        
        public async Task<ActorsDto> GetSearchedActors(string id,string procedureName, string connectionString, CancellationToken cancellationToken = default)
        {
            ActorsDto ActorDto = null;
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding the parameter to the command
                    command.Parameters.Add(new SqlParameter("@VariableId", SqlDbType.VarChar, 10) { Value = id });

                    await connection.OpenAsync();

                    // Executing the command and reading the result
                    await using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                             ActorDto = new ActorsDto
                            {
                                ActorID = reader.GetInt32(reader.GetOrdinal("ActorID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                FamilyName = reader.GetString(reader.GetOrdinal("FamilyName")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                DoB = reader.GetDateTime(reader.GetOrdinal("DoB")),
                                DoD = reader.IsDBNull(reader.GetOrdinal("DoD")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DoD")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender"))
                            };
                        }
                    }
                }
            }

            return ActorDto;
        }
    }
}
