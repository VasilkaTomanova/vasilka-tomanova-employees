
using EmployeesPairWork.Library;
using EmployeesPairWork.Models;
using System.Globalization;

namespace EmployeesPairWork.Services
{
    public class RenderViewService : IRenderViewService
    {
        public async Task<List<PairViewModel>> GetFilteredEmpoyees(List<CsvMappingModel> inputCollection)
        {
            List<PairViewModel> filteredEMployees = await FilterPairProjectEmployees(inputCollection);
            return filteredEMployees;
        }

        /// <summary>
        /// Filter aleady readed from file employees by project and time
        /// </summary>
        /// <param name="employeesFromFile"></param>
        /// <returns></returns>
        private async Task<List<PairViewModel>> FilterPairProjectEmployees(List<CsvMappingModel> employeesFromFile)
        {
            var groupedByProject = employeesFromFile.GroupBy(p => p.ProjectID.Trim(), (key, g) =>
                                                    new { ProjectID = key, ProjectEmployees = g.ToList() })
                                                    .Where(x => x.ProjectEmployees.Count >= Constants.MinValueForPair)
                                                    .ToList();

            List<PairViewModel> filteredEMployees = new List<PairViewModel>();
            foreach (var currProject in groupedByProject)
            {
                for (int i = 0; i < currProject.ProjectEmployees.Count; i++)
                {
                    CsvMappingModel currentFirstEmployee = currProject.ProjectEmployees[i];
                    string currentFirstEmployeeName = currentFirstEmployee.EmpID.Trim();
                    for (int j = i + 1; j < currProject.ProjectEmployees.Count; j++)
                    {
                        CsvMappingModel currentSecondEmployee = currProject.ProjectEmployees[j];
                        int commonTimeWork = await HasWorkAtSameTime(currentFirstEmployee, currentSecondEmployee);
                        if (commonTimeWork == Constants.NoPairWorkValue)
                        {
                            continue;
                        }
                        string currentSecondEmployeeName = currentSecondEmployee.EmpID.Trim();
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
                            CommonWorkDuration = commonTimeWork
                        });
                    }
                }
            }
            return filteredEMployees;
        }


        /// <summary>
        /// Return how many days given two employees work at same time of current project
        /// </summary>
        /// <param name="firstEmployee"></param>
        /// <param name="secondEmployee"></param>
        /// <returns></returns>
        private async Task<int> HasWorkAtSameTime(CsvMappingModel firstEmployee, CsvMappingModel secondEmployee)
        {
            DateTime firstEmployeeDateFrom;
            DateTime firstEmployeeDateTo;
            DateTime secondEmployeeFrom;
            DateTime secondEmployeeDateTo;

            DateTime.TryParseExact(firstEmployee.DateFrom.Trim(), DatetimeFormats.AllFormats, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out firstEmployeeDateFrom);
            if (firstEmployee.DateTo.ToLower() != Constants.NullValueForDateTo && firstEmployee.DateTo != Constants.EmptyValueForDateTo)
            {
                DateTime.TryParseExact(firstEmployee.DateTo.Trim(), DatetimeFormats.AllFormats, CultureInfo.InvariantCulture,
                                   DateTimeStyles.None, out firstEmployeeDateTo);
            }
            else
            {
                firstEmployeeDateTo = DateTime.Now.Date;
            }

            DateTime.TryParseExact(secondEmployee.DateFrom.Trim(), DatetimeFormats.AllFormats, CultureInfo.InvariantCulture,
                                  DateTimeStyles.None, out secondEmployeeFrom);
            if (secondEmployee.DateTo.ToLower() != Constants.NullValueForDateTo && secondEmployee.DateTo != Constants.EmptyValueForDateTo)
            {
                DateTime.TryParseExact(secondEmployee.DateTo.Trim(), DatetimeFormats.AllFormats, CultureInfo.InvariantCulture,
                                 DateTimeStyles.None, out secondEmployeeDateTo);
            }
            else 
            {
                secondEmployeeDateTo = DateTime.Now.Date;
            }
 
            //check if first employee work within second employee period
            if (firstEmployeeDateFrom >= secondEmployeeFrom && firstEmployeeDateFrom <= secondEmployeeDateTo)
            {
                if (firstEmployeeDateTo <= secondEmployeeDateTo)
                {
                    return (int)(firstEmployeeDateTo - firstEmployeeDateFrom).TotalDays + Constants.AddDayToCorrectValue;
                }
                return (int)(secondEmployeeDateTo - firstEmployeeDateFrom).TotalDays + Constants.AddDayToCorrectValue;
            }

            //check if second employee work within first employee period
            if (secondEmployeeFrom >= firstEmployeeDateFrom && secondEmployeeFrom <= firstEmployeeDateTo)
            {
                if (secondEmployeeDateTo <= firstEmployeeDateTo)
                {
                    return (int)(secondEmployeeDateTo - secondEmployeeFrom).TotalDays + Constants.AddDayToCorrectValue;
                }
                return (int)(firstEmployeeDateTo - secondEmployeeFrom).TotalDays + Constants.AddDayToCorrectValue;
            }

            return Constants.NoPairWorkValue;
        }

    }
}
