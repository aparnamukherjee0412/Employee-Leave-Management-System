using System;
using System.Collections.Generic;

namespace Infrastructure.DbModels;

public partial class Employee
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Department { get; set; }

    public string? Designation { get; set; }

    public DateOnly? JoinDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual User User { get; set; } = null!;
}
