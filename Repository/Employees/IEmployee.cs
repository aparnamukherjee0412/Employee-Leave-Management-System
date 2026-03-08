using Domain;
using Infrastructure.DbModels;

namespace Repository.Employees

{
    public interface IEmployee
    {
        Task<List<EmployeeViewModel>> GetEmployees(string search);
        Task AddEmployee(EmployeeViewModel model);
        Task<EmployeeViewModel> GetEmployeeById(int id);
        Task<EmployeeViewModel> GetEmployeeByUserId(int userid);
        Task UpdateEmployee(EmployeeViewModel model);
        Task DeactivateEmployee(int id);
    }
}
