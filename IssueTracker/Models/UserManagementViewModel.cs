using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using IssueTracker.Data;

namespace IssueTracker.Models
{
    public class UserManagementViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<ApplicationUser> OnProject { get; set; }
        public IEnumerable<ApplicationUser> NotOnProject { get; set; }
    }
}
