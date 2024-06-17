using System.Threading.Tasks;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Http;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represent business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest"> country object to add</param>
        /// <returns>Returns the country object after adding it (including 
        /// newly generated countryid) </returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);


        /// <summary>
        /// returns all countries from the list
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given country id
        /// </summary>
        /// <param name="countryID">CountryID (guid) to seacrh</param>
        /// <returns>Match country as countryResponse object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);

        /// <summary>
        /// Upload countries from excel file into the database
        /// </summary>
        /// <param name="formFile">Excel file with list of countries</param>
        /// <returns>returns number of countries added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
