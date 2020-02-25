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

            return View(await _context.Tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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