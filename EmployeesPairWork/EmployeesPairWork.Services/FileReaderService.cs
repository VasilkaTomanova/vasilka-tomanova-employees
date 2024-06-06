
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





        /// <summary>
        /// Filter aleady readed from file employees by project and time
        /// </summary>
        /// <param name="employeesFromFile"></param>
        /// <returns></returns>
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
                    CsvMappingModel currentFirstEmployee = currProject.ProjectEmployees[i];
                    string currentFirstEmployeeName = currentFirstEmployee.EmpID;
                    for (int j = i + 1; j < currProject.ProjectEmployees.Count; j++)
                    {
                        CsvMappingModel currentSecondEmployee = currProject.ProjectEmployees[j];    
                        if (!await HasWorkAtSameTime(currentFirstEmployee, currentSecondEmployee))
                        {
                            continue;
                        }
                        string currentSecondEmployeeName = currentSecondEmployee.EmpID;
                        PairViewModel? currentItemToAdd = filteredEMployees
                                                           .Where(x => (x.FirstEmployee == currentFirstEmployeeName && x.SecondEmployee == currentSecondEmployeeName)
                                                           || (x.FirstEmployee == currentSecondEmployeeName && x.SecondEmployee == currentFirstEmployeeName))
                                                           .FirstOrDefault();
                        if (currentItemToAdd == null)
                        {
                            currentItemToAdd = new PairViewModel();
                            currentItemToAdd.FirstEmployee = currentFirstEmployeeName;
                            currentItemToAdd.SecondEmployee = currentSecondEmployeeName;
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



        /// <summary>
        /// Return whether given two employees work at same time of current project
        /// </summary>
        /// <param name="firstEmployee"></param>
        /// <param name="secondEmployee"></param>
        /// <returns></returns>
        private async Task<bool> HasWorkAtSameTime(CsvMappingModel firstEmployee, CsvMappingModel secondEmployee)
        {
            DateTime firstEmployeeDateTo = firstEmployee.DateTo != "" ? DateTime.Parse(firstEmployee.DateTo) : DateTime.UtcNow;
            DateTime secondEmployeeDateTo = secondEmployee.DateTo != "" ? DateTime.Parse(secondEmployee.DateTo) : DateTime.UtcNow;

            if ((DateTime.Parse(firstEmployee.DateFrom) >= secondEmployeeDateTo
                && DateTime.Parse(firstEmployee.DateFrom) <= secondEmployeeDateTo)
                ||
                (DateTime.Parse(secondEmployee.DateFrom) >= firstEmployeeDateTo
                && DateTime.Parse(secondEmployee.DateFrom) <= firstEmployeeDateTo))
            {
                return true;
            }

            return false;
        }

    }
}
