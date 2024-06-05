
using EmployeesPairWork.Models;

namespace EmployeesPairWork.Services
{
    public interface IFileReaderService
    {
        /// <summary>
        /// Return all rows data from uploaded file
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<CsvMappingModel>> GetAllRowsFromFile(FileInputModel input);

    }
}
