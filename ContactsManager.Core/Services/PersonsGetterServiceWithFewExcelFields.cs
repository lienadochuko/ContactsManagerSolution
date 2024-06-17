using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.DTO;

namespace ContactsManager.Core.Services
{
    public class PersonsGetterServiceWithFewExcelFields : IPersonGetterServices
    {
        private readonly PersonGetterService _personGetterService;

        public PersonsGetterServiceWithFewExcelFields(PersonGetterService personGetterService)
        {
            _personGetterService = personGetterService;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personGetterService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            return await _personGetterService.GetFilteredPersons(searchBy, searchString);
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            return await _personGetterService.GetPersonByPersonID(personID);
        }

        public async Task<MemoryStream> GetPersonCSV()
        {
            MemoryStream memoryStream = new();
            StreamWriter streamWriter = new(memoryStream);


            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration, leaveOpen: true);

            //csvWriter.WriteHeader<PersonResponse>(); //using the model fields as heads such as PersonID, PersonName ...... 

            //await csvWriter.WriteRecordsAsync(persons);
            ////1, dan, dan@gmail.com ........

            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DOB));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.NIN));
            csvWriter.NextRecord(); //goes to the next line (\n)

            List<PersonResponse> persons = await GetAllPersons();

            foreach (PersonResponse person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                csvWriter.WriteField(person.DOB.HasValue ? person.DOB.Value.ToString("yyyy-MM-dd") : "");
                csvWriter.WriteField(person.Age);
                csvWriter.WriteField(person.Gender);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.NIN);
                csvWriter.NextRecord(); //goes to the next line (\n)
            }


            await csvWriter.FlushAsync(); //when the buffer in the stremwriter gets filled up it flushes to stream, At the end of writing,
                                          //you need to flush the writer so anything in the buffer gets written to the stream ensuring there is no missing record

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task<MemoryStream> GetPersonExcel()
        {
            MemoryStream memoryStream = new();

            using (ExcelPackage excelpackage = new(memoryStream))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "splash1.png");
                ExcelWorksheet workSheet = excelpackage.Workbook.Worksheets.Add("PersonsSheet");
                if (File.Exists(imagePath))
                {
                    // Add the image to the Excel file
                    FileInfo image = new(imagePath);
                    ExcelPicture picture = workSheet.Drawings.AddPicture("ImageName", image);
                    picture.SetSize(60);
                    picture.SetPosition(1, 0, 3, 0); // Adjust the position as needed
                }
                else
                {
                    // Handle the case when the image file does not exist
                    // You can log a message, throw an exception, etc.
                }

                using (ExcelRange excelRange = workSheet.Cells["A1:E1"])
                {
                    excelRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                }

                //Set Headings
                workSheet.Cells["A1"].Value = "Person Name";
                workSheet.Cells["B1"].Value = "Email";
                workSheet.Cells["C1"].Value = "Gender";
                //workSheet.Cells["D1"].Value = "Date of Birth";
                workSheet.Cells["D1"].Value = "Age";
                //workSheet.Cells["E1"].Value = "Address";
                workSheet.Cells["E1"].Value = "Country";


                //Align Headings
                workSheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["B1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //workSheet.Cells["F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //workSheet.Cells["G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //Bold
                workSheet.Cells["A1"].Style.Font.Bold = true;
                workSheet.Cells["B1"].Style.Font.Bold = true;
                workSheet.Cells["C1"].Style.Font.Bold = true;
                workSheet.Cells["D1"].Style.Font.Bold = true;
                workSheet.Cells["E1"].Style.Font.Bold = true;
                //workSheet.Cells["F1"].Style.Font.Bold = true;
                //workSheet.Cells["G1"].Style.Font.Bold = true;

                //Set Pattern and BackgroundColor 
                workSheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["B1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["C1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["D1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["E1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                //workSheet.Cells["F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //workSheet.Cells["F1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                //workSheet.Cells["G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //workSheet.Cells["G1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                int row = 4;

                List<PersonResponse> personResponses = await GetAllPersons();

                foreach (PersonResponse person in personResponses)
                {
                    workSheet.Cells[row, 1].Value = person.PersonName;
                    workSheet.Cells[row, 2].Value = person.Email;
                    workSheet.Cells[row, 3].Value = person.Gender;
                    //workSheet.Cells[row, 4].Value = person.DOB.HasValue ? person.DOB.Value.ToString("yyyy-MM-dd") : "";
                    workSheet.Cells[row, 4].Value = person.Age;
                    //workSheet.Cells[row, 5].Value = person.Address;
                    workSheet.Cells[row, 5].Value = person.Country;

                    workSheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    workSheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    workSheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    workSheet.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    workSheet.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    //workSheet.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    //workSheet.Cells[row, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    row++;
                }

                workSheet.Cells[$"A1:E{row}"].AutoFitColumns();

                await excelpackage.SaveAsync();

                memoryStream.Position = 0;
                return memoryStream;
            }
        }
    }
}
