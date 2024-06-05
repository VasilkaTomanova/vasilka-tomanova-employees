

using CsvHelper.Configuration.Attributes;

namespace EmployeesPairWork.Models
{
    public class CsvMappingModel
    {

        public string EmpID { get; set; }

        
        public string ProjectID { get; set; }


        public string DateFrom { get; set; }


        public string? DateTo { get; set; }
    }
}
