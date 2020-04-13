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
using Microsoft.AspNetCore.Authorization;
using TimeZoneConverter;

namespace IssueTracker.Controllers
{
    [Authorize(Roles = "Admin,Project Manager,Developer,Submitter")]
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

            var projectUsers = _context.ProjectUsers
                .Where(p => p.UserId == id);

            var ticketOut = new List<Ticket>();

            foreach(var item in projectUsers)
            {
                var ticketList = await _context.Tickets.Where(t => t.ProjectId == item.ProjectId)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.Project)
                    .ToListAsync().ConfigureAwait(false);

                ticketOut.AddRange(ticketList);
            }





            return View(ticketOut);


        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _context.Tickets
                .Include(m => m.TicketPriority)
                .Include(m => m.TicketStatus)
                .Include(m => m.TicketType)
                .Include(m => m.TicketComments)
                .Single(m => m.Id == id);

;
         


            if (ticket == null)
            {
                return NotFound();
            }



            //Ticket Properties
            var ticketDetails = new TicketDetailsViewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Created = ticket.Created,
                Updated = ticket.Updated,
                Description = ticket.Description,
                PercentComplete = ticket.PercentComplete,
                TicketComments = ticket.TicketComments
            
            };

            //ticketDetails.TicketStatus = status.Name;
            if (ticket.TicketStatusId != null)
            {
                ticketDetails.TicketStatus = ticket.TicketStatus.Name;
            }

            if (ticket.TicketPriorityId != null)
            {
                ticketDetails.TicketPriority = ticket.TicketPriority.Name;
            }


            if (ticket.TicketTypeId != null)
            {
                ticketDetails.TicketType = ticket.TicketType.Name;
            }

            var ownerName = _context.Users.Find(ticket.OwnerUserId);



            ticketDetails.OwnerName = ownerName.UserName;

            //Project Properties
            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == ticket.ProjectId).ConfigureAwait(false);

            ticketDetails.ProjectTitle = project.Title;
            ticketDetails.ProjectId = project.Id;


            return View(ticketDetails);


        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            var ticketView = new TicketCreateViewModel();
            var userId = _profileManager.CurrentUser.Id;
            //var projectList = new List<Project>();
            var ticketStatus = _context.TicketStatuses.ToList();

            var ticketPriority = _context.TicketPriorities.ToList();

            var ticketType = _context.TicketTypes.ToList();

            var projectList = _context.Projects.Where(x => x.ProjectUsers.Any(y => y.UserId == userId)).ToList();
            //var projectList = projects.ToList();


            ticketView.Projects = new SelectList(projectList, "Id", "Title");
            ticketView.TicketStatus = new SelectList(ticketStatus, "Id", "Name");
            ticketView.TicketTypes = new SelectList(ticketType, "Id", "Name");
            ticketView.TicketPriorities = new SelectList(ticketPriority, "Id", "Name");

            return View(ticketView);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,SelectedProject,SelectedStatus,SelectedPriority,SelectedType")] TicketCreateViewModel ticketView)
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
                    ticket.TicketStatusId = ticketView.SelectedStatus;
                    ticket.TicketPriorityId = ticketView.SelectedPriority;
                    ticket.TicketTypeId = ticketView.SelectedType;

                    _context.Tickets.Add(ticket);

                    await _context.SaveChangesAsync().ConfigureAwait(false);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _context.Tickets
                .Include(m => m.TicketPriority)
                .Include(m => m.TicketStatus)
                .Include(m => m.TicketType)
                .Single(m => m.Id == id);


            if (ticket == null)
            {
                return NotFound();
            }

            var ticketStatus = _context.TicketStatuses.ToList();

            var ticketPriority = _context.TicketPriorities.ToList();

            var ticketType = _context.TicketTypes.ToList();
            var ticketEdit = new TicketEditViewModel();

            if (ticket.TicketTypeId == null)
            {
                var getId = ticketType
                    .Single(m => m.Name == "Type Needed");
                ticketEdit.SelectedType = getId.Id;
            }
            else
            {
                ticketEdit.SelectedType = ticket.TicketType.Id;
            }

            if (ticket.TicketStatusId == null)
            {
                var getId = ticketStatus
                    .Single(m => m.Name == "Status Needed");
                ticketEdit.SelectedStatus = getId.Id;
            }
            else
            {
                ticketEdit.SelectedStatus = ticket.TicketStatus.Id;
            }

            if (ticket.TicketPriorityId == null)
            {
                var getId = ticketPriority
                    .Single(m => m.Name == "Priority Needed");
                ticketEdit.SelectedPriority = getId.Id;
            }
            else
            {
                ticketEdit.SelectedPriority = ticket.TicketPriority.Id;
            }

            //Ticket Properties

            ticketEdit.Id = ticket.Id;
            ticketEdit.Title = ticket.Title;
            ticketEdit.Created = ticket.Created;
            ticketEdit.Updated = ticket.Updated;
            ticketEdit.Description = ticket.Description;
            ticketEdit.PercentComplete = ticket.PercentComplete;
            ticketEdit.OwnerUserId = ticket.OwnerUserId;






            ticketEdit.TicketTypes = new SelectList(ticketType, "Id", "Name", ticketEdit.SelectedType);
            ticketEdit.TicketStatuses = new SelectList(ticketStatus, "Id", "Name", ticketEdit.SelectedStatus);
            ticketEdit.TicketPriorities = new SelectList(ticketPriority, "Id", "Name", ticketEdit.SelectedPriority);



            //Project Properties
            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == ticket.ProjectId).ConfigureAwait(false);

            ticketEdit.ProjectTitle = project.Title;

            return View(ticketEdit);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,Description,Created,Updated,PercentComplete,SelectedStatus,SelectedPriority,SelectedType")] TicketEditViewModel ticketEdit)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var ticket = await _context.Tickets
                        .FirstOrDefaultAsync(m => m.Id == ticketEdit.Id).ConfigureAwait(false);

                    ticket.Title = ticketEdit.Title;
                    ticket.Description = ticketEdit.Description;
                    ticket.Created = ticketEdit.Created;
                    ticket.Updated = DateTimeOffset.Now;
                    ticket.PercentComplete = ticketEdit.PercentComplete;
                    ticket.TicketStatusId = ticketEdit.SelectedStatus;
                    ticket.TicketPriorityId = ticketEdit.SelectedPriority;
                    ticket.TicketTypeId = ticketEdit.SelectedType;

                    _context.Update(ticket);
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

            return View(ticketEdit);
   
        }

        //Get: Tickets/CreateComment/5
        public ActionResult CreateComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _context.Tickets.Single(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }





            var ticketComment = new TicketComment
            {
                TicketId = ticket.Id

            };




            return View(ticketComment);
        }

        //Post: Tickets/CreateComment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("TicketId,Comment")]TicketComment ticketComment)
        {


            try
            {

                if (ModelState.IsValid)
                {


                    ticketComment.Created = DateTimeOffset.Now;

                    ticketComment.UserId = _profileManager.CurrentUser.Id;

                    _context.TicketComments.Add(ticketComment);

                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });

                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }


            return View(ticketComment);
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