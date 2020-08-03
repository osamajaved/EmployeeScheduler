using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeScheduler.Models
{
    public class Employee
    {
        public Employee() { UID = new Guid(); }
        [Key]
        public Guid UID { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        //public virtual ICollection<EmployeeShifts> EmployeeShifts { get; set; }
    }
}
