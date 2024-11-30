using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Views.Account;

namespace WebApplication1.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> RegisterUser(string username, string email, string password);
        Task<bool> IsEmailRegistered(string email);
        Task<bool> ValidateUser(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        //Task SavePasswordResetTokenAsync(int userId, string token);
        //Task<User> GetUserByPasswordResetTokenAsync(string token);
        //Task ClearPasswordResetTokenAsync(int userId);
        //Task UpdatePasswordAsync(int userId, string newPassword);
    }
}
