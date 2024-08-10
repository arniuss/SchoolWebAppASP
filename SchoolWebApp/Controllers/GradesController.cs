using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;

namespace SchoolWebApp.Controllers
{
    [Authorize]
    public class GradesController : Controller
    {
        private GradeService _gradeService;

        public GradesController(GradeService gradeService)
        {
            _gradeService = gradeService;
        }

        public async Task<IActionResult> CreateAsync()
		{
			await FillSelects();
			return View();
		}

		private async Task FillSelects()
		{
			var gradesDropDownData = await _gradeService.GetDropDownsData();
			ViewBag.Students = new SelectList(gradesDropDownData.Students, "Id", "LastName");
			ViewBag.Subjects = new SelectList(gradesDropDownData.Subjects, "Id", "Name");
		}

		[HttpPost]
        public async Task<IActionResult> Create(GradeDTO gradeDTO)
        {
            await _gradeService.CreateAsync(gradeDTO);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index()
        {
            var allGrades = await _gradeService.GetAllViewModelsAsync();
            return View(allGrades);
        }

        public async Task<IActionResult> Update(int id)
        {
            var gradeToEdit = await _gradeService.GetByIdAsync(id);
            if(gradeToEdit == null) return View("NotFound");
            var gradeDTO = _gradeService.ModelToDTO(gradeToEdit);
            return View(gradeDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, GradeDTO gradeDTO)
        {
            _gradeService.UpdateAsync(id, gradeDTO);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _gradeService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
