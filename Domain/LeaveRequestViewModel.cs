using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        [Required] public DateOnly FromDate { get; set; }
        [Required] public DateOnly ToDate { get; set; }
        [Required] public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime CreatedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Department { get; set; } = string.Empty;
    }
}
