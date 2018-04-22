using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpApp.Models
{
    [Table("Leaves")]
    public class Leave
    {
        [Key]
        public int LeaveId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string ApprovalStatus { get; set; }
        public Employee Employee { get; set; }
    }
}
