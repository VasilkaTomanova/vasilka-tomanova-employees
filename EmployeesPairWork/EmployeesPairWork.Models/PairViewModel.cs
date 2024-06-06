
using System.ComponentModel;

namespace EmployeesPairWork.Models
{
    public class PairViewModel
    {
        public PairViewModel()
        {
            this.CommonProjects = new List<CommonProjectVIewModel>();
        }

        [DisplayName("Employee 1")]
        public string FirstEmployee { get; set; }

        [DisplayName("Employee 2")]
        public string SecondEmployee { get; set; }

        public List<CommonProjectVIewModel> CommonProjects { get; set; }

    }
}
