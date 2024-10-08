using System;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml.Drawing;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Exceptions;

namespace ContactsManager.Core.Services
{
    public class MoviesUpdaterService(IMoviesRepository moviesRepository) : IMoviesUpdaterServices
    {
        /// <summary>
        /// Updates the actor's details
        /// </summary>
        /// <param name="actorsUpdateRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>returns truev if updated, else false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> UpdateActors(ActorsUpdateRequest actorsUpdateRequest, CancellationToken cancellationToken)
        {
            if (actorsUpdateRequest == null)
                throw new ArgumentNullException(nameof(actorsUpdateRequest));

            //validation
            ValidationHelper.ModelValidation(actorsUpdateRequest);

            //if (actorsUpdateRequest.ActorID == new Guid())
            //    throw new ArgumentException(nameof(personUpdateRequest.PersonID));

            bool isUpdated = await moviesRepository.UpdateActorsDetails(
                actorsUpdateRequest.ActorID,
                actorsUpdateRequest.FirstName,
                actorsUpdateRequest.FamilyName,
                actorsUpdateRequest.DOB,
                actorsUpdateRequest.DOD,
                actorsUpdateRequest.Gender,
                cancellationToken);

            if (isUpdated == false)
            {
                //throw new InvalidPersonIDException("Failed to update the actors details");
                return false;
            }


            return true;
        }
    }
}
