using SchoolWebApp.Models;

namespace SchoolWebApp.ViewModels
{
    public class GradesDropDownsViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public GradesDropDownsViewModel()
        {
            Students = new List<Student>();
            Subjects = new List<Subject>();
        }
    }
}
