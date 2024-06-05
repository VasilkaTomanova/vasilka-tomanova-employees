
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

            List<PairViewModel> filteredEMployees = await FilterPairProjectEmployees(employeesFromFile);
            return employeesFromFile;
        }


        private async Task<List<PairViewModel>> FilterPairProjectEmployees(List<CsvMappingModel> employeesFromFile)
        {
            var groupedByProject = employeesFromFile.GroupBy(p => p.ProjectID, (key, g) =>
                                                    new { ProjectID = key, ProjectEmployees = g.ToList() })
                                                    .ToList();

            List<PairViewModel> filteredEMployees = new List<PairViewModel>();

            foreach (var currProject in groupedByProject)
            {
                for (int i = 0; i < currProject.ProjectEmployees.Count; i++)
                {
                    string currentFirstEmployee = currProject.ProjectEmployees[i].EmpID;
                    for (int j = i+1; j < currProject.ProjectEmployees.Count; j++)
                    {
                        string currentSecondEmployee = currProject.ProjectEmployees[j].EmpID;
                        //TODO filter whether they work together!!!! NB
                        //!!!!!!!!!
                        PairViewModel? currentItemToAdd = filteredEMployees
                                                           .Where(x => (x.FirstEmployee == currentFirstEmployee && x.SecondEmployee == currentSecondEmployee)
                                                           || (x.FirstEmployee == currentSecondEmployee && x.SecondEmployee == currentFirstEmployee))
                                                           .FirstOrDefault();
                        if (currentItemToAdd == null)
                        {
                            currentItemToAdd = new PairViewModel();
                            currentItemToAdd.FirstEmployee = currentFirstEmployee;
                            currentItemToAdd.SecondEmployee = currentSecondEmployee;
                            filteredEMployees.Add(currentItemToAdd);
                        }
                        currentItemToAdd.CommonProjects.Add(new CommonProjectVIewModel
                        {
                            ProjectID = currProject.ProjectID,
                            //TODO common work time !!!!!! NB
                            CommonWorkDuration = 0
                        });
                    }
                }

            }

            return filteredEMployees;
        }


    }
}
