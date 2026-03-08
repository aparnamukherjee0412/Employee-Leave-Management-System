using Infrastructure.DbModels;
namespace Repository.Users
{
    public interface IUser
    {
        Task<User> ValidateUser(string email, string password);
    }
}
