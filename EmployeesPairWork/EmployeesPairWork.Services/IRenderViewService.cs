using EmployeesPairWork.Models;

namespace EmployeesPairWork.Services
{
    public interface IRenderViewService
    {

        /// <summary>
        /// Return filtered pair employees, wich work together on same project at same time
        /// </summary>
        /// <param name="inputCollection"></param>
        /// <returns></returns>
        public Task<List<PairViewModel>> GetFilteredEmpoyees(List<CsvMappingModel> inputCollection);
    }
}
