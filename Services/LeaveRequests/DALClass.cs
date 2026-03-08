using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.LeaveRequests;

namespace Services.LeaveRequests
{
    public class DALClass : ILeaveRequest
    {
        private readonly EmpLeaveManagementSystemContext _context;
        public DALClass(EmpLeaveManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveRequestViewModel>> GetAllLeaves()
        {
            var leaves = await _context.LeaveRequests
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.User)
                        .Select(x => new LeaveRequestViewModel
                        {
                            Id = x.Id,
                            EmployeeName = x.Employee.User.Name,
                            FromDate = x.FromDate,
                            ToDate = x.ToDate,
                            Reason = x.Reason,
                            Status = x.Status
                        })
                        .ToListAsync();

            return leaves;
        }
        public async Task<List<LeaveRequestViewModel>> GetEmployeeLeaves(int employeeId)
        {
            return await _context.LeaveRequests
                        .Where(x => x.EmployeeId == employeeId)
                        .Select(x => new LeaveRequestViewModel
                        {
                            Id = x.Id,
                            FromDate = x.FromDate,
                            ToDate = x.ToDate,
                            Reason = x.Reason,
                            Status = x.Status
                        })
                        .ToListAsync();
        }
        public async Task ApplyLeave(LeaveRequest leave)
        {
            leave.Status = "Pending";
            leave.CreatedDate = DateTime.Now;

            _context.LeaveRequests.Add(leave);

            await _context.SaveChangesAsync();
        }
        public async Task ApproveLeave(int id,int adminid)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            leave!.Status = "Approved";
            leave.ApprovedBy = adminid;
            leave.ApprovedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckOverlappingLeave(int employeeId, DateOnly from, DateOnly to)
        {
            return await _context.LeaveRequests.AnyAsync(x =>
            x.EmployeeId == employeeId &&
            x.Status != "Rejected" &&
            (from <= x.ToDate && to >= x.FromDate));
        }      
        public async Task RejectLeave(int id, int adminid)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            leave!.Status = "Rejected";
            leave.ApprovedBy = adminid;
            leave.ApprovedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
