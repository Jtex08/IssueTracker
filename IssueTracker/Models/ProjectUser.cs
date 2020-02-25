using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    public class ProjectUser
    {
        public int Id { get; set; }
        public int  ProjectId { get; set; }
        public string UserId { get; set; }
    }
}
