using SuperMarketApi.DTOs;
using SuperMarketApi.Models;

namespace SuperMarketApi.Services
{
    public interface IUserService
    {
        // User authentication
        string Login(LoginUserDto loginUserDto);
        void Register(CreateUserDto createUserDto);

        // Authorization
        bool Authorize(string token, RoleEnum[] roles);

        // Define user-related service methods here
        // Example:
        // IEnumerable<UserResponseDto> GetAllUsers();
        // UserResponseDto? GetUserById(int id);
        // UserResponseDto CreateUser(CreateUserDto createUserDto);
        // bool DeleteUser(int id);
    }
} 