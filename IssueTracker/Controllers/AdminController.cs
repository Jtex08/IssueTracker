using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.Data;
using IssueTracker.Models;
using Microsoft.AspNetCore.Authorization;



namespace IssueTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var adminIndex = new AdminViewModel
            {
                applicationUsers = _userManager.Users,
                ticketStatuses = _context.TicketStatuses,
                ticketPriorities = _context.TicketPriorities,
                ticketTypes = _context.TicketTypes
            };



            return View(adminIndex);
        }

        public ViewResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateViewModel user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.Name,
                    Email = user.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, "Submitter").ConfigureAwait(false);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", _userManager.Users);
        }

        public ViewResult CreateStatus() => View();

        [HttpPost]
        public async Task<IActionResult> CreateStatus(TicketStatus ticketStatus)
        {
            var status = new TicketStatus();
            status.Name = ticketStatus.Name;

            _context.TicketStatuses.Add(status);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.TicketStatuses.FindAsync(id);
            _context.TicketStatuses.Remove(status);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("Index");
        }

        public ViewResult CreateType() => View();

        [HttpPost]
        public async Task<IActionResult> CreateType(TicketType ticketType)
        {
            var type = new TicketType();
            type.Name = ticketType.Name;

            _context.TicketTypes.Add(type);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> DeleteType(int id)
        {
            var status = await _context.TicketTypes.FindAsync(id);
            _context.TicketTypes.Remove(status);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("Index");
        }


        public ViewResult CreatePriority() => View();

        [HttpPost]
        public async Task<IActionResult> CreatePriority(TicketPriority ticketPriority)
        {
            var priority = new TicketPriority();
            priority.Name = ticketPriority.Name;

            _context.TicketPriorities.Add(priority);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> DeletePriority(int id)
        {
            var priority = await _context.TicketPriorities.FindAsync(id);
            _context.TicketPriorities.Remove(priority);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToAction("Index");
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }


}
