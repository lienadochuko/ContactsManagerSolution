
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace ContactsManager.Core.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<ActorsDto>> GetAllActors(CancellationToken cancellationToken);
    }


    public class MoviesService(IMoviesRepository moviesRepository) : IMoviesService
    {
        public async Task<IEnumerable<ActorsDto>> GetAllActors(CancellationToken cancellationToken = default)
        {
            var Actors = await moviesRepository.GetActors(cancellationToken);

            return Actors;
        }
    }
}