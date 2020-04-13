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
using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.Controllers
{
    [Authorize(Roles = "Admin,Project Manager,Developer,Submitter")]
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
            var user = _profileManager.CurrentUser;
            if (_profileManager.IsInRole("Admin"))
            {
                var projects = _context.Projects;

                return View(await projects.ToListAsync().ConfigureAwait(false));
            }
            else
            {
                var projects = _context.Projects.Where(x => x.ProjectUsers.Any(y => y.UserId == id));

                View(await projects.ToListAsync().ConfigureAwait(false));

            }

            return View();
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
                .Include(p => p.Tickets).ThenInclude(t => t.TicketPriority)
                .Include(p => p.ProjectUsers)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

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
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    ProjectUser projUser = new ProjectUser();

                    projUser.UserId = _profileManager.CurrentUser.Id;
                    projUser.ProjectId = project.Id;

                    _context.ProjectUsers.Add(projUser);
                    await _context.SaveChangesAsync().ConfigureAwait(false);

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
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
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
                    await _context.SaveChangesAsync().ConfigureAwait(false);
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
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
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

        // GET: Projects/UserManagement/5
        [Authorize(Roles ="Admin, Project Manager")]
        public async Task<IActionResult> UserManagement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            List<ApplicationUser> OnProject = new List<ApplicationUser>();
            List<ApplicationUser> NotOnProject = new List<ApplicationUser>();

            OnProject = UsersOnProject(id);

            NotOnProject = UsersNotOnProject(id);



            return View(new UserManagementViewModel{

                OnProject = OnProject,
                NotOnProject = NotOnProject,
                Project = project
            
            });
        }

        //Post: Projects/UserManagement/5
        [HttpPost]
        public async Task<IActionResult> UserManagement(ProjectUserModification model)
        {
            var projectUsers = _context.ProjectUsers.Where(p => p.ProjectId == model.ProjectId);
            var projectUsersList = projectUsers.ToList();

            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    var projectUser = new ProjectUser();
                    projectUser.ProjectId = model.ProjectId;
                    projectUser.UserId = userId;
                    _context.ProjectUsers.Add(projectUser);
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    var ToBeRemoved = projectUsersList.Where(p => p.UserId == userId).Single();

                    _context.ProjectUsers.Remove(ToBeRemoved);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                                       
                }
            }


            return RedirectToAction("Details", "Projects", new { id = model.ProjectId });


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
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private List<ApplicationUser> UsersOnProject(int? projectId)
        {

            var queryDb = _context.ProjectUsers
                .Where(p => p.ProjectId == projectId).Select(p => p.User);

            var usersOnProject= queryDb.ToList();           

            return usersOnProject;

        }

        private List<ApplicationUser> UsersNotOnProject(int? projectId)
        {
            var usersOnProject = UsersOnProject(projectId);

            var queryDb = _context.Users;

            var allUsers = queryDb.ToList();

            var usersNotOnProject = allUsers.Except(usersOnProject).ToList();

            return usersNotOnProject;
        }
    }
}