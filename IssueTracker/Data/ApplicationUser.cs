using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Models;




namespace IssueTracker.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<TicketComment> TicketComments { get; set; }

        public ApplicationUser()
        {
        }
    
    }
 
}
