using Domain;
using Infrastructure.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Repository.Employees;
using Repository.LeaveRequests;
using Repository.Report;

namespace MainProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEmployee _employee;
        private readonly ILeaveRequest _leave;
        private readonly IReport _report;
        private readonly IHubContext<MainProject.NotificationHub.NotificationHub> _hub;
        public AdminController(IEmployee employee, ILeaveRequest leave, IReport report, IHubContext<MainProject.NotificationHub.NotificationHub> hub)
        {
            _employee = employee;
            _leave = leave;
            _report = report;
            _hub = hub;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var dashboard = await _report.GetAdminDashboard();

            return View(dashboard);
        }

        #region EMPLOYEE MANAGEMENT
        public async Task<IActionResult> Employees(string search)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var employees = await _employee.GetEmployees(search);

            return View(employees);
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _employee.AddEmployee(model);

            return RedirectToAction("Employees");
        }

        public async Task<IActionResult> EditEmployee(int id)
        {
            var emp = await _employee.GetEmployeeById(id);

            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeViewModel model)
        {
            await _employee.UpdateEmployee(model);

            return RedirectToAction("Employees");
        }

        public async Task<IActionResult> DeactivateEmployee(int id)
        {
            await _employee.DeactivateEmployee(id);

            return RedirectToAction("Employees");
        }
        #endregion
        
        #region LEAVE MANAGEMENT
        public async Task<IActionResult> LeaveRequests()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var leaves = await _leave.GetAllLeaves();

            return View(leaves);
        }

        public async Task<IActionResult> ApproveLeave(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            await _leave.ApproveLeave(id,userId);
            await _hub.Clients.All.SendAsync("ReceiveNotification",
        "Your leave request has been Approved.");
            return RedirectToAction("LeaveRequests");
        }

        public async Task<IActionResult> RejectLeave(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            await _leave.RejectLeave(id,userId);
            await _hub.Clients.All.SendAsync("ReceiveNotification",
        "Your leave request has been Rejected.");
            return RedirectToAction("LeaveRequests");
        }
        #endregion
        
        #region REPORTS
        public async Task<IActionResult> Reports(string department, string status)
        {
            var report = await _report.GetLeaveReport(department, status);

            return View(report);
        }
        public async Task<IActionResult> ExportExcel()
        {
            var file = await _report.ExportLeaveReport();

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "LeaveReport.xlsx");
        }
        #endregion
    }
}
