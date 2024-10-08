using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating actor entity
    /// </summary>
    public interface IMoviesUpdaterServices
    {
        Task<bool> UpdateActors(ActorsUpdateRequest actorsUpdateRequest, CancellationToken cancellationToken);
    }
}
