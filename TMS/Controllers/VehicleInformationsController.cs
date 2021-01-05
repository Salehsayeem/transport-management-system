using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMS.Data;
using TMS.Models;
using TMS.Models.ViewModel;

namespace TMS.Controllers
{
    public class VehicleInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        RoleManager<IdentityRole> _roleManager;
        public VehicleInformationsController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: VehicleInformations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VehicleInformation.Include(v => v.Route).Include(v => v.VehicleType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VehicleInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleInformation = await _context.VehicleInformation
                .Include(v => v.Route)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicleInformation == null)
            {
                return NotFound();
            }

            return View(vehicleInformation);
        }

        // GET: VehicleInformations/Create
        public IActionResult Create()
        {
            var u = new UserRoleMapping();
            var joinRoleUser = (from user in _context.ApplicationUsers
                join role in _context.UserRoles on user.Id equals role.UserId
                join us in _context.Roles on role.RoleId equals us.Id
                where us.Name == "Driver"
                select new SelectListItem()
                {
                    Value = user.Id,
                    Text = user.UserName
                }).ToList();
                                          //var join = _context.Roles.Where(a => a.Name == "Driver").ToList();
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId");
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "VehicleTypeId", "VehicleTypeId");
            //ViewBag.UserId = new SelectList(_context.Roles.Where(a => a.Name == "Driver"), "Id", "Name");
            joinRoleUser.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.UserId = joinRoleUser;
           // ViewBag.UserId = new SelectList(joinroleuser, Value, Text);
            return View();
        }

        // POST: VehicleInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,VehicleNo,SeatCapacity,VehicleTypeId,RouteId,UserId")] VehicleInformation vehicleInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicleInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var join = from a in _context.ApplicationUsers
                       join v in _context.UserRoles on a.Id equals v.UserId
                       select a.UserName;
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId", vehicleInformation.RouteId);
            ViewData["UserId"] = new SelectList(join, "Id", "UserName", vehicleInformation.UserId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "VehicleTypeId", "VehicleTypeId", vehicleInformation.VehicleTypeId);
            return View(vehicleInformation);
        }

        // GET: VehicleInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleInformation = await _context.VehicleInformation.FindAsync(id);
            if (vehicleInformation == null)
            {
                return NotFound();
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId", vehicleInformation.RouteId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "VehicleTypeId", "VehicleTypeId", vehicleInformation.VehicleTypeId);
            return View(vehicleInformation);
        }

        // POST: VehicleInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleId,VehicleNo,SeatCapacity,VehicleTypeId,RouteId,UserId")] VehicleInformation vehicleInformation)
        {
            if (id != vehicleInformation.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleInformationExists(vehicleInformation.VehicleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId", vehicleInformation.RouteId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "VehicleTypeId", "VehicleTypeId", vehicleInformation.VehicleTypeId);
            return View(vehicleInformation);
        }

        // GET: VehicleInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleInformation = await _context.VehicleInformation
                .Include(v => v.Route)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicleInformation == null)
            {
                return NotFound();
            }

            return View(vehicleInformation);
        }

        // POST: VehicleInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleInformation = await _context.VehicleInformation.FindAsync(id);
            _context.VehicleInformation.Remove(vehicleInformation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleInformationExists(int id)
        {
            return _context.VehicleInformation.Any(e => e.VehicleId == id);
        }
    }
}
