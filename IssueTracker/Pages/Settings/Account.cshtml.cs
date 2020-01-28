using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.Data;
using IssueTracker.Services.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IssueTracker.Pages.Settings
{
    public class AccountModel : PageModel
    {
        public AccountModel() 
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}