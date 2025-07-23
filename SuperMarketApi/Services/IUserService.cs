using SuperMarketApi.DTOs;
using SuperMarketApi.DTOs.User;
using SuperMarketApi.Models;
using System.Threading.Tasks;

namespace SuperMarketApi.Services
{
    public interface IUserService
    {
        // User authentication
        Task<string> LoginAsync(LoginUserDto loginUserDto);
        Task RegisterAsync(CreateUserDto createUserDto);
        Task UpdatePersonalInfoAsync(int userId, UpdateUserInfoDto updateDto);
        Task ChangeUserRoleAsync(ChangeUserRoleDto dto);
        Task<UserPersonalInfoDto?> GetPersonalInfoAsync(int userId);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);

    }
} 