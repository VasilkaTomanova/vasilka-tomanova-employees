
namespace EmployeesPairWork.Models
{
    public class PairViewModel
    {
        public PairViewModel()
        {
            this.CommonProjects = new List<CommonProjectVIewModel>();
        }
        public string FirstEmployee { get; set; }

        public string SecondEmployee { get; set; }

        public List<CommonProjectVIewModel> CommonProjects { get; set; }

    }
}
