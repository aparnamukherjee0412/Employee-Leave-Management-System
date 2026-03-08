using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]public string Name { get; set; } = string.Empty;
        [Required] public string Department { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        [Required] public string? Designation { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")] 
        public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        [Required] public DateOnly? JoinDate { get; set; }
    }
}
