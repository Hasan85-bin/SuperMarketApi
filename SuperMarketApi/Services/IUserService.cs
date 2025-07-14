using SuperMarketApi.DTOs;
using SuperMarketApi.Models;

namespace SuperMarketApi.Services
{
    public interface IUserService
    {
        // Define user-related service methods here
        // Example:
        // IEnumerable<UserResponseDto> GetAllUsers();
        // UserResponseDto? GetUserById(int id);
        // UserResponseDto CreateUser(CreateUserDto createUserDto);
        // bool DeleteUser(int id);

        // Authorization method
        bool Authorize(string token, RoleEnum[] roles);
    }
} 