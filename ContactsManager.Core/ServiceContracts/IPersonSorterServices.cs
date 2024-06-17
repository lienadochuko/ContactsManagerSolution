using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ContactsManager.Core.Enums;
using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonSorterServices
    {
        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allPersons">Represents the list of persons to sort</param>
        /// <param name="sortBy">Name of the property (key), based on which the 
        /// list of persons should be sorted</param>
        /// <param name="sortOrderOptions">ASC or DESC</param>
        /// <returns>Returns sorted list of persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons,
            string sortBy, SortOrderOptions sortOrderOptions);

    }
}
