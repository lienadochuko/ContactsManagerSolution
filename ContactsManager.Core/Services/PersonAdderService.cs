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

namespace ContactsManager.Core.Services
{
    public class PersonAdderService : IPersonAdderServices
    {
        //private field
        //private readonly List<Person> _persons;
        private readonly IPersonRepository _personRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ICountriesService _countriesService;
        //private readonly ILogger<PersonAdderService> _logger;

        public PersonAdderService(IPersonRepository personRepository,
            ICountriesRepository countriesRepository)
        {
            //_persons = new List<Person>();
            //ILogger<PersonAdderService> logger

            _personRepository = personRepository;
            _countriesRepository = countriesRepository;
            //_logger = logger;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //Validate personAddRequest
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model Validate PersonName
            ValidationHelper.ModelValidation(personAddRequest);


            //Convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate personID
            person.PersonID = Guid.NewGuid();

            //add person to person list
            await _personRepository.AddPerson(person);
            //_db.sp_InsertPerson(person);

            //convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }

        public async Task<int> UploadPersonsFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream);
            int personInserted = 0;


            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Persons"];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(worksheet.Cells[row, 1].Value);
                    string? cellValue1 = Convert.ToString(worksheet.Cells[row, 2].Value);
                    string? cellValue2 = Convert.ToString(worksheet.Cells[row, 3].Value);
                    string? cellValue2M = Convert.ToString(worksheet.Cells[row, 4].Value);
                    string? cellValue2D = Convert.ToString(worksheet.Cells[row, 5].Value);
                    string? cellValue3 = Convert.ToString(worksheet.Cells[row, 6].Value);
                    string? cellValue4 = Convert.ToString(worksheet.Cells[row, 7].Value);
                    string? cellValue5 = Convert.ToString(worksheet.Cells[row, 8].Value);
                    string? cellValue6 = Convert.ToString(worksheet.Cells[row, 9].Value);
                    string? cellValue7 = Convert.ToString(worksheet.Cells[row, 10].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? personName = cellValue;
                        string? personEmail = cellValue1;
                        string? personDOB = cellValue2 + "-" + cellValue2M + "-" + cellValue2D;
                        string? personGender = cellValue3;
                        string? personaddress = cellValue4;
                        string? personCountry = cellValue5;
                        string? personRecieve = cellValue6;
                        string? personNin = cellValue7;

                        DateTime? date = DateTime.ParseExact(personDOB, "yyyy-M-dd", null, DateTimeStyles.None);
                        bool recieve;

                        if (personRecieve == "True")
                        { recieve = true; }
                        else
                        {
                            recieve = false;
                        }

                        if (await _personRepository.GetPersonByPersonName(personName) == null)
                        {
                            Guid ID = Guid.NewGuid();

                            Person person = new Person()
                            {
                                PersonID = Guid.NewGuid(),
                                PersonName = personName,
                                Email = personEmail,
                                DOB = date,
                                Gender = personGender,
                                Address = personaddress,
                                country = new Country()
                                {
                                    CountryID = ID,
                                    CountryName = personCountry,
                                },
                                CountryID = ID,
                                RecieveNewsLetter = recieve,
                                NIN = personNin,

                            };
                            Country country = await _countriesRepository.GetCountryByCountryName(personCountry);
                            if (country == null)
                            {
                                await _countriesRepository.AddCountry(person.country);
                                await _personRepository.AddPerson(person);


                                personInserted++;
                            }
                            else
                            {
                                personInserted = 0;
                            }
                        }
                    }
                }
            }

            return personInserted;
        }
    }
}
