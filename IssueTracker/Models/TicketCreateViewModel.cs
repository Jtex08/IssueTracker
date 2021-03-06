﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Models
{
    public class TicketCreateViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        [Display(Name = "Project")]
        public SelectList Projects { get; set; }

        [Display(Name = "Type")]
        public SelectList TicketTypes { get; set; }

        [Display(Name = "Priority")]
        public SelectList TicketPriorities { get; set; }


        [Display(Name = "Status")]
        public SelectList TicketStatus { get; set; }

        [Required(ErrorMessage = "Please select a project")]
        public int SelectedProject { get; set; }


        [Required(ErrorMessage = "Please select a Status")]
        public int SelectedStatus { get; set; }


        [Required(ErrorMessage = "Please select a type")]
        public int SelectedType { get; set; }
        [Required(ErrorMessage = "Please select a priority")]
        public int SelectedPriority { get; set; }
    }
}
