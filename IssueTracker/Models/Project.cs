using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using IssueTracker.Data;

namespace IssueTracker.Models
{
    public class Project
    {
        public int Id { get; set; } //Id of project       

        //[Display(Name = "Owner")]
        //public string OwnerUserId { get; set; }        // Foreign Key - Owner of Ticket 

        //Unique properties
        [Required(ErrorMessage = "Title is required.")]
        [Display(Name = "Title")]
        public string Title { get; set; }              // Title of the ticket

        [Display(Name = "Description")]
        public string Description { get; set; }        // Description of the ticket

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }



        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        //public virtual ICollection<ApplicationUser> Users { get; set; }
        //public virtual ICollection<Ticket> Tickets { get; set; }

        //public Project()
        //{
        //    Users = new HashSet<ApplicationUser>();
        //    Tickets = new HashSet<Ticket>();
        //}
    }
}
