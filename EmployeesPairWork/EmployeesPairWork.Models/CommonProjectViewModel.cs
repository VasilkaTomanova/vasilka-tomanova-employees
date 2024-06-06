using System.ComponentModel;

namespace EmployeesPairWork.Models
{
    public class CommonProjectVIewModel
    {
        [DisplayName("Project")]
        public string ProjectID { get; set; }

        [DisplayName("Work days together")]
        public int CommonWorkDuration { get; set; }
    }
}
