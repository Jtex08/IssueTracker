using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.Data;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using IssueTracker.Services.Profile;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;       
        private readonly ProfileManager _profileManager;


        public TicketsController(ApplicationDbContext context, ProfileManager profileManager)
        {
            _context = context;
            _profileManager = profileManager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var id = _profileManager.CurrentUser.Id;
            var tickets = _context.Tickets.Where(x => x.OwnerUserId == id);
            return View(await tickets.ToListAsync());

            //return View(await _context.Tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            var ticketView = new TicketCreateViewModel();
            var userId = _profileManager.CurrentUser.Id;
            var projectList = new List<Project>();

            var projects = _context.Projects.Where(x => x.ProjectUsers.Any(y => y.UserId == userId));
            projectList = projects.ToList();

            ticketView.Projects = new SelectList(projectList, "Id", "Title");

            return View(ticketView);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,SelectedProject")] TicketCreateViewModel ticketView)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var ticket = new Ticket();
                    ticket.Title = ticketView.Title;
                    ticket.Description = ticketView.Description;
                    ticket.ProjectId = ticketView.SelectedProject;
                    ticket.OwnerUserId = _profileManager.CurrentUser.Id;
                    ticket.Created = DateTimeOffset.Now;
                    _context.Tickets.Add(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }



            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }

            return View(ticketView);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tickets/Edit/5
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

        // GET: Tickets/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tickets/Delete/5
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