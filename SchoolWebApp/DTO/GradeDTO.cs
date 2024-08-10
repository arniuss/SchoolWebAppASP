using SchoolWebApp.Models;
using System.ComponentModel;

namespace SchoolWebApp.DTO
{
    public class GradeDTO
    {
        public int Id { get; set; }
        [DisplayName("Student Name")]
        public int StudentId { get; set; }
        [DisplayName("Subject")]
        public int SubjectId { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        [DisplayName("Grade")]
        public int Mark { get; set; }
    }
}
