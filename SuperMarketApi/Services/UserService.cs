using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SuperMarketApi.DTOs;
using SuperMarketApi.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using SuperMarketApi.DTOs.User;
using SuperMarketApi.Repositories;

namespace SuperMarketApi.Services
{
    public class UserService : IUserService
    {
        // Private fields - these hold our data and dependencies
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        // private readonly IHttpContextAccessor _httpContextAccessor; // Removed as it's unused
        // private readonly IUserRepository _userRepository; // Removed as it's now part of IUnitOfWork

        // Constructor - this is where we initialize our service
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            // _users = new List<User>(); // Remove in-memory list usage
            // _lastId = 2;
            // Seed data - adding initial users for testing
            // SeedUsers();
        }

        // Private method for seeding initial data
        // private void SeedUsers()
        // {
        //     var user1 = new User
        //     {
        //         ID = 1,
        //         UserName = "JohnDoe",
        //         Email = "john.doe@example.com",
        //         Phone = "123-456-7890",
        //         Role = RoleEnum.Customer,
        //         Password = HashPassword("password123"), // Set a hashed password
        //         ShoppingCart = new List<User.OrderItem>(),
        //         Purchases = new List<Purchase>()
        //     };
        //
        //     var user2 = new User
        //     {
        //         ID = 2,
        //         UserName = "JaneSmith",
        //         Email = "jane.smith@example.com",
        //         Phone = "098-765-4321",
        //         Role = RoleEnum.Admin,
        //         Password = HashPassword("adminpass"), // Set a hashed password
        //         ShoppingCart = new List<User.OrderItem>(),
        //         Purchases = new List<Purchase>()
        //     };
        //
        //     _users.Add(user1);
        //     _users.Add(user2);
        // }


        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task RegisterAsync(CreateUserDto createUserDto)
        {
            if (await _unitOfWork.Users.ExistsByUserNameAsync(createUserDto.UserName))
                throw new BadHttpRequestException("Username already in use.");
            if (await _unitOfWork.Users.ExistsByEmailAsync(createUserDto.Email))
                throw new BadHttpRequestException("Email already in use.");
            if (!string.IsNullOrEmpty(createUserDto.Phone) && await _unitOfWork.Users.ExistsByPhoneAsync(createUserDto.Phone))
                throw new BadHttpRequestException("Phone already in use.");
            var user = _mapper.Map<User>(createUserDto);
            user.Password = HashPassword(createUserDto.Password);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(loginUserDto.UserName);
            if (user == null)
            {
                throw new BadHttpRequestException("Invalid UserName: UserName not found");
            }
            var hashedPassword = HashPassword(loginUserDto.Password);
            if (user.Password != hashedPassword)
            {
                throw new BadHttpRequestException("Invalid Password: Password is incorrect");
            }

            // JWT token generation
            // These values should match those in Program.cs
            // IMPORTANT: The key must be at least 32 characters (256 bits) for HS256
            var jwtKey = "YourSuperSecretKey1234567890!@#$%^"; // 32+ chars
            var jwtIssuer = "SuperMarketApi";
            var jwtAudience = "SuperMarketApiUsers";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims for the user (ID as NameIdentifier)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            // Return the serialized JWT token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task UpdatePersonalInfoAsync(int userId, UpdateUserInfoDto updateDto)
        {
            if (await _unitOfWork.Users.ExistsByEmailAsync(updateDto.Email, userId))
                throw new BadHttpRequestException("Email already in use by another user.");
            if (!string.IsNullOrEmpty(updateDto.Phone) && await _unitOfWork.Users.ExistsByPhoneAsync(updateDto.Phone, userId))
                throw new BadHttpRequestException("Phone already in use by another user.");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new BadHttpRequestException("Invalid User ID");
            }
            _mapper.Map(updateDto, user);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeUserRoleAsync(ChangeUserRoleDto dto)
        {
            // Try to parse as enum name (case-insensitive)
            RoleEnum roleEnum;
            if (Enum.TryParse<RoleEnum>(dto.Role, true, out roleEnum))
            {
                // Parsed as string name
            }
            // If not, try to parse as int
            else if (int.TryParse(dto.Role, out int roleInt) && Enum.IsDefined(typeof(RoleEnum), roleInt))
            {
                roleEnum = (RoleEnum)roleInt;
            }
            else
            {
                throw new BadHttpRequestException("Invalid role value.");
            }

            var user = await _unitOfWork.Users.GetByUserNameAsync(dto.UserName);
            if (user == null)
            {
                throw new BadHttpRequestException("User not found.");
            }
            user.Role = roleEnum;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            if (dto.NewPassword != dto.RepeatNewPassword)
                throw new BadHttpRequestException("New password and repeated password do not match.");
            if (dto.NewPassword.Length < 6)
                throw new BadHttpRequestException("Password must be at least 6 characters long.");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new BadHttpRequestException("User not found.");
            var currentHashed = HashPassword(dto.CurrentPassword);
            if (user.Password != currentHashed)
                throw new BadHttpRequestException("Current password is incorrect.");
            user.Password = HashPassword(dto.NewPassword);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<UserPersonalInfoDto?> GetPersonalInfoAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return null;
            return new UserPersonalInfoDto
            {
                ID = user.ID,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role.ToString()
            };
        }




        // TODO: Implement all the interface methods here for User!
        // We'll add them step by step as you learn
    }
}