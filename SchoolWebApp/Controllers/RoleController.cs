using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Models;

namespace SchoolWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
	{
		private RoleManager<IdentityRole> _roleManager;
		private UserManager<AppUser> _userManager;
		
		public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View(_roleManager.Roles.OrderBy(role => role.Name));
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateAsync(string name)
		{
			if(ModelState.IsValid)
			{
				IdentityResult identityResult = await _roleManager.CreateAsync(new IdentityRole { Name = name });
				if(identityResult.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					AddErrors(identityResult);
				}
			}
			return View(name);
		}
		public async Task<IActionResult> Update(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			var users = await _userManager.Users.ToListAsync();
			List<AppUser> members = new List<AppUser>();
			List<AppUser> nonmembers = new List<AppUser>();
			foreach (AppUser user in users)
			{
				var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
				list.Add(user);
			}
			return View(new RoleEdit
			{
				Role = role,
				RoleMembers = members,
				RoleNonMembers = nonmembers
			});
		}
		[HttpPost]
		public async Task<IActionResult> Delete(string id) {
			var roleToDelete = await _roleManager.FindByIdAsync(id);
			if(roleToDelete != null)
			{
				IdentityResult result = await _roleManager.DeleteAsync(roleToDelete);
				if(result.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					AddErrors(result);
				}
			}
			ModelState.AddModelError("", "No role found");
			return View("Index", _roleManager.Roles);
		}
		[HttpPost]
		public async Task<IActionResult> Update(RoleModifications roleModifications)
		{
			AppUser user;
			foreach (var id in roleModifications.AddIds ?? new string[] { })
			{
				user = await _userManager.FindByIdAsync(id);
				if (user != null)
				{
					IdentityResult result = await _userManager.AddToRoleAsync(user, roleModifications.RoleName);
					if(result != IdentityResult.Success)
					{
						AddErrors(result);
					}
				}
			}
			foreach(var id in roleModifications.DeleteIds ?? [])
			{
				user = await _userManager.FindByIdAsync(id);
				if(user != null)
				{
					IdentityResult result = await _userManager.RemoveFromRoleAsync(user, roleModifications.RoleName);
					if(result != IdentityResult.Success)
					{
						AddErrors(result);
					}
				}
			}
			return RedirectToAction("Index");
		}
		private void AddErrors(IdentityResult identityResult)
		{
			foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
		}
	}
}
