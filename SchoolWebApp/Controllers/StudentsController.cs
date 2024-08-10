using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;

namespace SchoolWebApp.Controllers
{
	[Authorize]
	public class StudentsController : Controller
	{
		private StudentService _studentService;

		public StudentsController(StudentService studentService)
		{
			_studentService = studentService;
		}

		public IActionResult Index()
		{
			IEnumerable<StudentDTO> allStudents = _studentService.GetAllStudents();
			return View(allStudents);
		}
		[Authorize(Roles = "Teacher, Admin")]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
        public async Task<IActionResult> CreateAsync(StudentDTO studentDTO)
        {
			await _studentService.AddStudentAsync(studentDTO);
            return RedirectToAction("Index");
        }
		
		public async Task<IActionResult> EditAsync(int id)
		{
			var studentToEdit = await _studentService.GetByIdAsync(id);
			if(studentToEdit == null)
			{
				return View("NotFound");
			}
			return View(studentToEdit);
		}
		[HttpPost]
		public async Task<IActionResult> EditAsync(StudentDTO studentDTO, int id)
		{
			await _studentService.EditAsync(id, studentDTO);
			return RedirectToAction("Index");
		}

		[HttpDelete]
		[Authorize(Roles = "Teacher, Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var studentToDelete = await _studentService.GetByIdAsync(id);
			if(studentToDelete == null)
			{
				return View("NotFound");
			}
			await _studentService.DeleteAsync(id);
			return RedirectToAction("Index");
		}
    }
}
