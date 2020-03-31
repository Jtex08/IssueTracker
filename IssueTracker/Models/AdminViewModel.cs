using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.Data;
using IssueTracker.Models;

namespace IssueTracker.Models
{
    public class AdminViewModel
    {
       public IEnumerable<ApplicationUser> applicationUsers { get; set; }
       public IEnumerable<TicketStatus> ticketStatuses { get; set; }

    }
}
