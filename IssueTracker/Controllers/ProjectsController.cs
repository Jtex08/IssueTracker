using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Data;
using Microsoft.AspNetCore.Identity;
using IssueTracker.Models;
using System.Data;
using System.Security.Claims;
using IssueTracker.Services.Profile;

namespace IssueTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProfileManager _profileManager;

        public ProjectsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ProfileManager profileManager)
        {
            _userManager = userManager;
            _context = context;
            _profileManager = profileManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var id = _profileManager.CurrentUser.Id;
            var projects = _context.Projects.Where(x => x.ProjectUsers.Any(y => y.UserId == id));
            return View(await projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Tickets)                
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Project project)
        {
            //var userIDD = _profileManager.CurrentUser.Id;
            //project.Users.Add(_profileManager.CurrentUser);
            //project.OwnerUserId = _profileManager.CurrentUser.Id;
            //var thisUser = _context.Users.FindAsync(userIDD);
            //project.Users.Add(thisUser);

            //var tuck = _userManager.GetUserId(User);

            project.Created = DateTimeOffset.Now;
            //project.OwnerUserId = userIDD;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();

                    ProjectUser projUser = new ProjectUser();

                    projUser.UserId = _profileManager.CurrentUser.Id;
                    projUser.ProjectId = project.Id;

                    _context.ProjectUsers.Add(projUser);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(project);
        }


        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    project.Updated = DateTimeOffset.Now;
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}