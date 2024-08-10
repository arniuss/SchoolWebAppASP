using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebApp.DTO
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        [MaxLength(25)]
        [MinLength(4)]
        public string Name { get; set; }
    }
}
