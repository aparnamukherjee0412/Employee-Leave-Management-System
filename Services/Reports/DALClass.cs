using Domain;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.Report;

namespace Services.Reports
{
    public class DALClass : IReport
    {
        private readonly EmpLeaveManagementSystemContext _context;
        public DALClass(EmpLeaveManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardViewModel> GetAdminDashboard()
        {
            return new AdminDashboardViewModel
            {
                TotalEmployees = await _context.Employees.CountAsync(x => x.IsActive),

                PendingLeaves = await _context.LeaveRequests
                 .CountAsync(x => x.Status == "Pending"),

                ApprovedLeaves = await _context.LeaveRequests
                 .CountAsync(x => x.Status == "Approved"),

                RejectedLeaves = await _context.LeaveRequests
                 .CountAsync(x => x.Status == "Rejected")
            };
        }
        public async Task<EmployeeDashboardViewModel> GetEmployeeDashboard(int employeeId)
        {
            var leaves = await _context.LeaveRequests
           .Where(x => x.EmployeeId == employeeId)
           .ToListAsync();

            return new EmployeeDashboardViewModel
            {
                TotalLeaves = leaves.Count,

                PendingLeaves = leaves.Count(x => x.Status == "Pending"),

                ApprovedLeaves = leaves.Count(x => x.Status == "Approved"),

                RejectedLeaves = leaves.Count(x => x.Status == "Rejected"),

                MyLeaves = leaves.Select(x => new LeaveRequestViewModel
                {
                    Id = x.Id,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Reason = x.Reason,
                    Status = x.Status
                }).ToList()
            };
        }
        public async Task<List<LeaveRequestViewModel>> GetLeaveReport(string department, string status)
        {
            var query = _context.LeaveRequests
           .Include(x => x.Employee)
           .AsQueryable();

            if (!string.IsNullOrEmpty(department))
                query = query.Where(x => x.Employee.Department == department);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(x => x.Status == status);

            return await query.Select(x => new LeaveRequestViewModel
            {
                EmployeeName = x.Employee.User.Name,
                Department = x.Employee.Department!,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                Status = x.Status
            }).ToListAsync();
        }
        public async Task<byte[]> ExportLeaveReport()
        {
            var data = await GetLeaveReport(null, null);

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var sheet = workbook.Worksheets.Add("Leave Report");

            sheet.Cell(1, 1).Value = "Employee";
            sheet.Cell(1, 2).Value = "Department";
            sheet.Cell(1, 3).Value = "From Date";
            sheet.Cell(1, 4).Value = "To Date";
            sheet.Cell(1, 5).Value = "Status";

            int row = 2;

            foreach (var item in data)
            {
                sheet.Cell(row, 1).Value = item.EmployeeName;
                sheet.Cell(row, 2).Value = item.Department;
                sheet.Cell(row, 3).Value = item.FromDate.ToString("dd-MM-yyyy");
                sheet.Cell(row, 4).Value = item.ToDate.ToString("dd-MM-yyyy");
                sheet.Cell(row, 5).Value = item.Status;

                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

    }
}
