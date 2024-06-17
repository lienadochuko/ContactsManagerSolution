using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonGetterServices
    {


        /// <summary>
        /// Returns all person
        /// </summary>
        /// <returns>Returns a list of object of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personID"></param>
        /// <returns>Return matching person object </returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Return the list of all person object that
        /// matches the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Return the list of all person object that
        /// matches the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);


        /// <summary>
        /// Returns the person as CSV
        /// </summary>
        /// <returns>REturns the memory stream as CSV Data</returns>
        Task<MemoryStream> GetPersonCSV();


        /// <summary>
        /// Returns the person as Excel
        /// </summary>
        /// <returns>REturns the memory stream as Excel Data of persons</returns>
        Task<MemoryStream> GetPersonExcel();
    }
}
