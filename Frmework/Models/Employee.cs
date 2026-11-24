using System;
using System.ComponentModel.DataAnnotations;

namespace Frmework.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

}
