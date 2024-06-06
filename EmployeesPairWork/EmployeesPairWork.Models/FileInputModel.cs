
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EmployeesPairWork.Models
{
    public class FileInputModel
    {
        public FileInputModel()
        {
            this.Employees = new List<PairViewModel>();
        }

        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }


        public List<PairViewModel> Employees { get; set; }

    }
}
