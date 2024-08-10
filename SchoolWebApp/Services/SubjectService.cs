using Microsoft.EntityFrameworkCore;
using SchoolWebApp.DTO;
using SchoolWebApp.Models;

namespace SchoolWebApp.Services
{
    public class SubjectService
    {
        private ApplicationDbContext _applicationDbContext;

        public SubjectService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<SubjectDTO> GetAllSubjects()
        {
            var allSubjects = _applicationDbContext.Subjects.ToList();
            var subjectsDtos = new List<SubjectDTO>();
            foreach (var subject in allSubjects)
            {
                subjectsDtos.Add(ModelToDTO(subject));
            }
            return subjectsDtos;
        }
        public async Task AddSubjectAsync(SubjectDTO subjectDto)
        {
            await _applicationDbContext.Subjects.AddAsync(DtoToModel(subjectDto));
            await _applicationDbContext.SaveChangesAsync();
        }
        public async Task<SubjectDTO> GetByIdAsync(int id)
        {
            var subject = await VerifyExistence(id);
            return ModelToDTO(subject);
        }
        public async Task EditAsync(int id, SubjectDTO subjectDTO)
        {
            _applicationDbContext.Subjects.Update(DtoToModel(subjectDTO));
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var subjectToDelete = await _applicationDbContext.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            //if(studentToDelete == null)
            //{
            //    return null;
            //}
            _applicationDbContext.Subjects.Remove(subjectToDelete);
            await _applicationDbContext.SaveChangesAsync();
        }
        private async Task<Subject> VerifyExistence(int id)
        {
            var subject = await _applicationDbContext.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null)
            {
                return null;
            }
            return subject;
        }
        private static SubjectDTO ModelToDTO(Subject subject)
        {
            return new SubjectDTO
            {
                Id = subject.Id,
                Name = subject.Name
            };
        }
        private static Subject DtoToModel(SubjectDTO subjectDTO)
        {
            return new Subject
            {
                Id = subjectDTO.Id,
                Name = subjectDTO.Name
            };
        }
    }
}
