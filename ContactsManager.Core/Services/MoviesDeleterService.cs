﻿using System;
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
    public class MoviesDeleterService(IMoviesRepository moviesRepository) : IMoviesDeleterServices
    {
        /// <summary>
        /// Updates the actor's details
        /// </summary>
        /// <param name="actorsUpdateRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>returns truev if updated, else false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> DeleteActors(string actorID, CancellationToken cancellationToken)
        {
            if (actorID == null)
                throw new ArgumentNullException(nameof(actorID));

            //if (actorsUpdateRequest.ActorID == new Guid())
            //    throw new ArgumentException(nameof(personUpdateRequest.PersonID));

            bool isDeleted = await moviesRepository.DeleteActor(actorID, cancellationToken);

            if (isDeleted == false)
            {
                //throw new InvalidPersonIDException("Failed to update the actors details");
                return false;
            }


            return true;
        }
    }
}
