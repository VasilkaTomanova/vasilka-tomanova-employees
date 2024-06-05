
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
            List<CsvMappingModel> customerDtos = new List<CsvMappingModel>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // TrimOptions = TrimOptions.Trim
                MissingFieldFound = null,
                Delimiter = ", ",
                HeaderValidated = null,
                // PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            using (var reader = new StreamReader(input.FormFile.OpenReadStream()))

            using (var csvr = new CsvReader(reader, config))
            {
                //  csvr.Read();
                //  csvr.ReadHeader();
                customerDtos = csvr.GetRecords<CsvMappingModel>().ToList();

                return customerDtos;
                // var customers = _mapper.Map<IEnumerable<Customer>>(customerDtos);
            }
        }
    }
}
