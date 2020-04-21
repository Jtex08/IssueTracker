using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.Data;
using IssueTracker.Services.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IssueTracker.Services.Profile;

namespace IssueTracker.Pages.Settings
{
    public class AccountModel : PageModel
    {

        private readonly ProfileManager _profileManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ProfileManager profileManager) 
        {
            _profileManager = profileManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var user = _profileManager.CurrentUser;
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync().ConfigureAwait(false);
                    return RedirectToPage("/Index");
                }
                else
                    return Page();
            }
            else return Page();
        }
    }
}