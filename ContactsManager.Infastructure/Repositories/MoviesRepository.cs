using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Helpers;
using ContactsManager.Infastructure.Repositories.DataAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Infastructure.Repositories
{
    public class MoviesRepository(IDataRepository dataRepository, IConfiguration configuration) : IMoviesRepository
    {
        public async Task<IEnumerable<ActorsDto>> GetActors(CancellationToken cancellationToken)
        {
            return await dataRepository.GetListOfActors("dbo.GetAllActors", CustomHelpers.GetConnectionString(configuration, "SecondConnection"), null, reader =>
            {
                return new ActorsDto
                {
                    ActorID = CustomHelpers.GetSafeInt32(reader, 0),
                    FirstName = CustomHelpers.GetSafeString(reader, 1),
                    FamilyName = CustomHelpers.GetSafeString(reader, 2),
                    FullName = CustomHelpers.GetSafeString(reader, 3),
                    DoB = CustomHelpers.GetDateTime(reader, 4),
                    DoD = CustomHelpers.GetDateTime(reader, 5),
                    Gender = CustomHelpers.GetSafeString(reader, 6)
                };
            }, cancellationToken);
        }
    }

}
