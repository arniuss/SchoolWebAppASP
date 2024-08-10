using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolWebApp.DTO;
using SchoolWebApp.Models;

namespace SchoolWebApp.Services
{
	public class StudentService
	{
		private ApplicationDbContext _applicationDbContext;

		public StudentService(ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public IEnumerable<StudentDTO> GetAllStudents()
		{
			var allStudents = _applicationDbContext.Students.ToList();
			var studentsDtos = new List<StudentDTO>();
			foreach (var student in allStudents)
            {
                studentsDtos.Add(ModelToDTO(student));
            }
            return studentsDtos;
		}
        public async Task AddStudentAsync(StudentDTO studentDto)
        {
            await _applicationDbContext.Students.AddAsync(DtoToModel(studentDto));
            await _applicationDbContext.SaveChangesAsync();
        }
        public async Task<StudentDTO> GetByIdAsync(int id)
        {
            var student = await VerifyExistence(id);
			return ModelToDTO(student);
        }     
        public async Task EditAsync(int id, StudentDTO studentDTO)
        {
            _applicationDbContext.Students.Update(DtoToModel(studentDTO));
            await _applicationDbContext.SaveChangesAsync();
        }	
        public async Task DeleteAsync(int id)
        {
            var studentToDelete = await _applicationDbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
            //if(studentToDelete == null)
            //{
            //    return null;
            //}
            _applicationDbContext.Students.Remove(studentToDelete);
            await _applicationDbContext.SaveChangesAsync();     
        }
        private async Task<Student> VerifyExistence(int id)
        {
            var student = await _applicationDbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return null;
            }
            return student;
        }
        private static StudentDTO ModelToDTO(Student student)
        {
            return new StudentDTO
            {
                DateOfBirth = student.DateOfBirth,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Id = student.Id
            };
        }        
        private static Student DtoToModel(StudentDTO studentDto)
        {
            return new Student
            {
                DateOfBirth = studentDto.DateOfBirth,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Id = studentDto.Id
            };
        }
    }
}
