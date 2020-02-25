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
            return View(await _context.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            project.OwnerUserId = _profileManager.CurrentUser.Id;
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Projects/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}