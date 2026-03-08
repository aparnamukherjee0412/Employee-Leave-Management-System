using Domain;
using Infrastructure.DbModels;
using Microsoft.AspNetCore.Mvc;
using Repository.Employees;
using Repository.LeaveRequests;
using Repository.Report;

namespace MainProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILeaveRequest _leave;
        private readonly IReport _report;
        private readonly IEmployee _emp;

        public EmployeeController(ILeaveRequest leave, IReport report,IEmployee employee)
        {
            _leave = leave;
            _report = report;
            _emp = employee;
        }

        private bool IsEmployee()
        {
            return HttpContext.Session.GetString("Role") == "Employee";
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!IsEmployee())
                return RedirectToAction("Login", "Account");

            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var emp = await _emp.GetEmployeeByUserId(userId);
            int empId = emp.Id;

            var dashboard = await _report.GetEmployeeDashboard(empId);

            return View(dashboard);
        }

        #region APPLY LEAVE
        public IActionResult ApplyLeave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLeave(LeaveRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.FromDate > model.ToDate)
            {
                ModelState.AddModelError("", "From Date cannot be greater than To Date");
                return View(model);
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var emp = await _emp.GetEmployeeByUserId(userId);
            if (emp == null)
            {
                ModelState.AddModelError("", "Employee not found");
                return View(model);
            }

            int empId = emp.Id;
            bool overlap = await _leave.CheckOverlappingLeave(empId, model.FromDate, model.ToDate);

            if (overlap)
            {
                ModelState.AddModelError("", "Leave already exists in this date range");
                return View(model);
            }

            await _leave.ApplyLeave(new LeaveRequest
            {
                EmployeeId = empId,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                Reason = model.Reason
            });

            return RedirectToAction("MyLeaves");
        }
        #endregion

        #region VIEW LEAVES        
        public async Task<IActionResult> MyLeaves()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var emp = await _emp.GetEmployeeByUserId(userId);            
            int empId = emp.Id;
            var leaves = await _leave.GetEmployeeLeaves(empId);
            return View(leaves);
        }
        #endregion
    }
}
