using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Repository.Report
{
    public interface IReport
    {
        Task<AdminDashboardViewModel> GetAdminDashboard();

        Task<EmployeeDashboardViewModel> GetEmployeeDashboard(int employeeId);

        Task<List<LeaveRequestViewModel>> GetLeaveReport(string department, string status);

        Task<byte[]> ExportLeaveReport();
    }
}
