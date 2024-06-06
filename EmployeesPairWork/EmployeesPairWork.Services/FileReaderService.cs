
using CsvHelper;
using CsvHelper.Configuration;
using EmployeesPairWork.Models;
using System.Globalization;

namespace EmployeesPairWork.Services
{
    public class FileReaderService : IFileReaderService
    {
        public async Task<List<CsvMappingModel>> GetAllRowsFromFile(FileInputModel input)
        {

            List<CsvMappingModel> employeesFromFile = new List<CsvMappingModel>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                Delimiter = ", ",
                HeaderValidated = null,
            };

            using (var reader = new StreamReader(input.FormFile.OpenReadStream()))

            using (var csvr = new CsvReader(reader, config))
            {
                employeesFromFile = csvr.GetRecords<CsvMappingModel>().OrderBy(x => x.ProjectID).ThenBy(x => x.EmpID).ToList();
            }

            return employeesFromFile;
        }

    }
}
