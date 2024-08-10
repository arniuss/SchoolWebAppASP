using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;

namespace SchoolWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsApiController : ControllerBase
    {
        private SubjectService _subjectService;

        public SubjectsApiController(SubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<SubjectDTO>> Index()
        {
            var allSubjects = _subjectService.GetAllSubjects();
            return Ok(allSubjects);
        }
    }
}
