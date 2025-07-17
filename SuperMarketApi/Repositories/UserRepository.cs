using SuperMarketApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuperMarketApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SuperMarketDbContext _context;
        public UserRepository(SuperMarketDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ID == id);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByUserNameAsync(string userName, int? currentUserId = null)
        {
            if (currentUserId == null)
            {
                // Registration: check if any user has this username
                return await _context.Users.AnyAsync(u => u.UserName == userName);
            }
            else
            {
                // Update: check if any other user has this username
                return await _context.Users.AnyAsync(u => u.UserName == userName && u.ID != currentUserId.Value);
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? currentUserId = null)
        {
            if (currentUserId == null)
            {
                // Registration: check if any user has this email
                return await _context.Users.AnyAsync(u => u.Email == email);
            }
            else
            {
                // Update: check if any other user has this email
                return await _context.Users.AnyAsync(u => u.Email == email && u.ID != currentUserId.Value);
            }
        }

        public async Task<bool> ExistsByPhoneAsync(string phone, int? currentUserId = null)
        {
            if (currentUserId == null)
            {
                // Registration: check if any user has this phone
                return await _context.Users.AnyAsync(u => u.Phone == phone);
            }
            else
            {
                // Update: check if any other user has this phone
                return await _context.Users.AnyAsync(u => u.Phone == phone && u.ID != currentUserId.Value);
            }
        }
    }
} 