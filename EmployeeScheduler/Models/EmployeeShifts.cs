using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeScheduler.Models
{
    public class EmployeeShifts
    {
        public EmployeeShifts()
        {
            UID = Guid.NewGuid();
        }
        [Key]
        public Guid UID { get; set; }
        public Guid EmployeeUID { get; set; }
        [ForeignKey("EmployeeUID")]
        public virtual Employee Employee { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
