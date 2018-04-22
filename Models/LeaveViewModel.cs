using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpApp.Models
{
    public class LeaveViewModel
    {
        public int LeaveId { get; set; }
        public String EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApprovalStatus { get; set; }
    }
}
