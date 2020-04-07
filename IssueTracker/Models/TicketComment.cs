using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset Created { get; set;} 
        public int TicketId { get; set; }
        public string UserId { get; set; }


    }
}
