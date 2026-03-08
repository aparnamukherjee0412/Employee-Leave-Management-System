namespace Domain
{
    public class EmployeeDashboardViewModel
    {
        public int TotalLeaves { get; set; }
        public int PendingLeaves { get; set; }
        public int ApprovedLeaves { get; set; }
        public int RejectedLeaves { get; set; }
        public List<LeaveRequestViewModel> MyLeaves { get; set; }
    }
}
