using Domain;
using Infrastructure.DbModels;

namespace Repository.LeaveRequests
{
    public interface ILeaveRequest
    {
        Task ApplyLeave(LeaveRequest leave);
        Task<List<LeaveRequestViewModel>> GetEmployeeLeaves(int employeeId);
        Task<List<LeaveRequestViewModel>> GetAllLeaves();
        Task ApproveLeave(int id, int adminid);
        Task RejectLeave(int id, int adminid);
        Task<bool> CheckOverlappingLeave(int employeeId, DateOnly from, DateOnly to);
    }
}
