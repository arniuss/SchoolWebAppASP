using Microsoft.EntityFrameworkCore;
using SchoolWebApp.DTO;
using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;

namespace SchoolWebApp.Services
{
    public class GradeService
    {
        private ApplicationDbContext _dbContext;

        public GradeService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        public async Task<GradesDropDownsViewModel> GetDropDownsData()
        {
            var gradesDropDownsData = new GradesDropDownsViewModel()
            {
                Students = await _dbContext.Students.OrderBy(student => student.LastName).ToListAsync(),
                Subjects = await _dbContext.Subjects.OrderBy(subject => subject.Name).ToListAsync()
            };
            return gradesDropDownsData;
        }

		internal async Task CreateAsync(GradeDTO gradeDTO)
		{
			Grade gradeToInsert = await DTOToModel(gradeDTO);
			await _dbContext.Grades.AddAsync(gradeToInsert);
			await _dbContext.SaveChangesAsync();
		}

		private async Task<Grade> DTOToModel(GradeDTO gradeDTO)
		{
			return new Grade()
			{
				Date = DateTime.Today,
				Mark = gradeDTO.Mark,
				Topic = gradeDTO.Topic,
				Student = _dbContext.Students.FirstOrDefault(student => student.Id == gradeDTO.StudentId),
				Subject = _dbContext.Subjects.FirstOrDefault(subject => subject.Id == gradeDTO.SubjectId)

			};
		}

		public async Task<IEnumerable<GradesViewModel>> GetAllViewModelsAsync()
        {
            List<Grade> grades = await _dbContext.Grades.Include(gr => gr.Student).Include(gr => gr.Subject).ToListAsync();
            List<GradesViewModel> gradesViewModel = new List<GradesViewModel>();
            foreach (Grade grade in grades)
            {
                gradesViewModel.Add(new GradesViewModel
                {
                    Date = grade.Date,
                    Id = grade.Id,
                    Mark = grade.Mark,
                    StudentName = $"{grade.Student.LastName} {grade.Student.FirstName}",
                    SubjectName = grade.Subject.Name,
                    Topic = grade.Topic
                });
            }
            return gradesViewModel;
        }

		internal async Task<Grade> GetByIdAsync(int id)
		{
			return await _dbContext.Grades.Include(gr => gr.Student).Include(gr => gr.Subject).FirstOrDefaultAsync(grade => grade.Id == id);
		}

        internal GradeDTO ModelToDTO(Grade grade)
        {
            return new GradeDTO
            {
                Date = grade.Date,
                Id = grade.Id,
                Mark = grade.Mark,
                StudentId = grade.Student.Id,
                SubjectId = grade.Subject.Id,
                Topic = grade.Topic
            };
        }

		internal async void UpdateAsync(int id, GradeDTO gradeDTO)
		{
			Grade updatedGrade = await DTOToModel(gradeDTO);
            _dbContext.Grades.Update(updatedGrade);
            await _dbContext.SaveChangesAsync();
		}

		internal async Task DeleteAsync(int id)
		{
			var gradeToDelete = await _dbContext.Grades.FirstOrDefaultAsync(g => g.Id == id);
	        _dbContext.Grades.Remove(gradeToDelete);
            await _dbContext.SaveChangesAsync();

		}
	}   
}
