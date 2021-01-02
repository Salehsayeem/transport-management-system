using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TMS.Data;
using TMS.Models.ViewModel;

namespace TMS.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;
        public RoleController(RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.roles = roles;
            return View(roles);
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.Msg = "The Role is already exist";
                return View();
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["Save"] = "Role has been saved successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<ActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            ViewBag.Id = role.Id;
            ViewBag.Name = role.Name;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.Msg = "The Role is already exist";
                ViewBag.Name = name;
                return View();
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["Save"] = "Role has been updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        public async Task<IActionResult> Assign()
        {
            ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Assign(RoleViewModel roleUser)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == roleUser.UserId);
            var isCheckRoleAssign = await _userManager.IsInRoleAsync(user, roleUser.RoleId);
            if (isCheckRoleAssign)
            {
                ViewBag.mgs = "This user already assign this role.";
                ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return View();
            }
            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);
            if (role.Succeeded)
            {
                TempData["save"] = "User Role assigned.";
                return RedirectToAction(nameof(AssignUserRole));
            }
            return View();
        }
        public ActionResult AssignUserRole()
        {
            var result = from u in _db.UserRoles
                         join r in _db.Roles on u.RoleId equals r.Id
                         join a in _db.ApplicationUsers on u.UserId equals a.Id
                         select new UserRoleMapping()
                         {
                             UserId = u.UserId,
                             RoleId = u.RoleId,
                             UserName = a.UserName,
                             RoleName = r.Name
                         };
            ViewBag.UserRole = result;
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["delete"] = "Role has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

    }
}
