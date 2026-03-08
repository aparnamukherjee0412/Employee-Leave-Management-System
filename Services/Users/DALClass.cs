using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.Users;

namespace Services.Users
{
    public class DALClass : IUser
    {
        private readonly EmpLeaveManagementSystemContext _context;
        public DALClass(EmpLeaveManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<User> ValidateUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email &&
                                                            x.Password == password && x.IsActive);
        }
    }
}
