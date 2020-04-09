using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using IssueTracker.Data;

namespace IssueTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }     //Id of ticket

        //[Required(ErrorMessage = "Project is required.")]
        //[Display(Name = "Project")]
        public int ProjectId { get; set; }             // Foreign Key - Project the ticket belongs to

        [Display(Name = "Owner")]
        public string OwnerUserId { get; set; } //Foreign Key-Owner or submitter of ticket

        [Display(Name = "Status")]
        public int? TicketStatusId { get; set; }    //Foreign Key-Ticket Status

        [Display(Name = "Priority")]
        public int? TicketPriorityId { get; set; }    //Foreign Key-Ticket Priority

        [Display(Name = "Type")]
        public int? TicketTypeId { get; set; }    //Foreign Key-Ticket Type

        //Unique properties
        [Required(ErrorMessage = "Title is required.")]
        [Display(Name = "Title")]
        public string Title { get; set; }              // Title of the ticket

        [Display(Name = "Description")]
        public string Description { get; set; }        // Description of the ticket

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name = "% Complete")]
        public int PercentComplete { get; set; }       // Percent Complete

        [Display(Name = "Created")]
        public DateTimeOffset Created { get; set; }    // When the ticket was added into the system

        [Display(Name = "Updated")]
        public DateTimeOffset? Updated { get; set; }   // Last time the ticket was updated in the system

        [Display(Name = "Archived")]
        public bool Archived { get; set; }

        // nav properties 
        public TicketStatus TicketStatus { get; set; }
        public TicketType TicketType { get; set; }
        public TicketPriority TicketPriority { get; set; }

        public ICollection<TicketComment> TicketComments { get; set; }


        //nav-parent
        public virtual ApplicationUser OwnerUser { get; set; }
        //public virtual Project Project { get; set; }


    }
}
