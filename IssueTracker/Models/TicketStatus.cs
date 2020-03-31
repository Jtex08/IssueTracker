﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // nav
        public virtual ICollection<Ticket> Tickets { get; set; }
        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}
