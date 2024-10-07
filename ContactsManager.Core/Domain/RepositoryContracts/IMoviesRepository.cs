using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.Domain.RepositoryContracts
{
   /// <summary>
    /// Represent data access logic for Managing Movies Data
    /// </summary>
    public interface IMoviesRepository
    {
        /// <summary>
        /// get all the actors object to the data store
        /// </summary>
        /// <param name="cancellationToken">CancellationToken cancellationToken</param>
        /// <returns>Returns the list of Actors objects</returns>
        Task<IEnumerable<ActorsDto>> GetAllActors(CancellationToken cancellationToken);


        /// <summary>
        /// get an the actors object to the data store
        /// </summary>
        /// <param name="cancellationToken">CancellationToken cancellationToken</param>
        /// <returns>Returns the Actor object</returns>
        Task<ActorsDto> GetActors(string id,CancellationToken cancellationToken);
    }
}
