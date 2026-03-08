using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.Employees;

namespace Services.Employees
{
    public class DALClass : IEmployee
    {
        private readonly EmpLeaveManagementSystemContext _context;
        public DALClass(EmpLeaveManagementSystemContext context)
        {
            _context = context;
        }

        // Get Employees 
        public async Task<List<EmployeeViewModel>> GetEmployees(string search)
        {
            var query =from e in _context.Employees
                        join u in _context.Users
                            on e.UserId equals u.Id
                        where e.IsActive
                        select new { e, u };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.u.Name.Contains(search) ||
                    x.e.Designation!.Contains(search) ||
                    x.e.Department!.Contains(search));
            }

            return await query
                .Select(x => new EmployeeViewModel
                {
                    Id = x.e.Id,
                    UserId = x.e.UserId,
                    Name = x.u.Name,
                    Department = x.e.Department!,
                    Designation=x.e.Designation!,
                    JoinDate=x.e.JoinDate
                })
                .ToListAsync();
        }

        // Add Employee
        public async Task AddEmployee(EmployeeViewModel model)
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Role = "Employee",
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var employee = new Employee
            {
                UserId = user.Id,
                Department = model.Department,
                Designation = model.Designation,
                JoinDate = model.JoinDate,
                IsActive = true
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();
        }

        // Get Employee By Id
        public async Task<EmployeeViewModel> GetEmployeeById(int id)
        {
            var emp = await _context.Employees
                   .Include(x => x.User)
                   .FirstOrDefaultAsync(x => x.Id == id);

            if (emp == null)
                return null;

            return new EmployeeViewModel
            {
                Id = emp.Id,
                UserId = emp.UserId,
                Name = emp.User.Name,
                Email = emp.User.Email,
                Department = emp.Department!,
                Designation = emp.Designation,
                JoinDate = emp.JoinDate,
                IsActive = emp.IsActive
            };
        }
        public async Task<EmployeeViewModel?> GetEmployeeByUserId(int userId)
        {
            return await _context.Employees
                .Where(x => x.UserId == userId)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.User.Name,
                    Email = x.User.Email,
                    Department = x.Department!,
                    Designation = x.Designation,
                    JoinDate = x.JoinDate,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();
        }

        // Update Employee
        public async Task UpdateEmployee(EmployeeViewModel model)
        {
            var employee = await _context.Employees
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (employee != null)
            {
                // Update user table
                employee.User.Name = model.Name;
                employee.User.Email = model.Email;
                employee.User.Password = model.Password;

                // Update employee table
                employee.Department = model.Department;
                employee.Designation = model.Designation;
                employee.JoinDate = model.JoinDate;

                await _context.SaveChangesAsync();
            }
        }

        // Deactivate Employee
        public async Task DeactivateEmployee(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                employee.IsActive = false;

                await _context.SaveChangesAsync();
            }
        }
    }
}
