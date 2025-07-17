using SuperMarketApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperMarketApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUserNameAsync(string userName);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> ExistsByUserNameAsync(string userName, int? currentUserId = null);
        Task<bool> ExistsByEmailAsync(string email, int? currentUserId = null);
        Task<bool> ExistsByPhoneAsync(string phone, int? currentUserId = null);
    }
} 