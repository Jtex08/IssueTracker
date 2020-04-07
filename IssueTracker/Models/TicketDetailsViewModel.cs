using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Models
{
    public class TicketDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }


        [Display(Name = "Project")]
        public string ProjectTitle { get; set; }

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name = "% Complete")]
        public int PercentComplete { get; set; }       // Percent Complete

        [Display(Name = "Type")]
        public string? TicketType { get; set; }
        [Display(Name = "Priority")]
        public string? TicketPriority { get; set; }

        [Display(Name = "Status")]
        public string? TicketStatus { get; set; }

        [Display(Name = "Owner")]
        public string OwnerName { get; set; }
        [Display(Name = "Assigned To")]
        public string AssignedToUserName { get; set; }

        public ICollection<TicketComment> TicketComments { get; set; }
    }
}
