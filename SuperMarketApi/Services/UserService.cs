using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SuperMarketApi.DTOs;
using SuperMarketApi.Models;
using System.Security.Cryptography;
using System.Text;

namespace SuperMarketApi.Services
{
    public class UserService : IUserService
    {
        // Private fields - these hold our data and dependencies
        private readonly ICollection<User> _users;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private int _lastId;
        private int _nextPurchaseId;

        // Constructor - this is where we initialize our service
        public UserService(IProductService productService, IMapper mapper)
        {
            _users = new List<User>();
            _productService = productService;
            _mapper = mapper;
            _lastId = 2;
            // Seed data - adding initial users for testing
            SeedUsers();
        }

        // Private method for seeding initial data
        private void SeedUsers()
        {
            var user1 = new User
            {
                ID = 1,
                UserName = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890",
                Role = RoleEnum.Customer
            };

            var user2 = new User
            {
                ID = 2,
                UserName = "Jane Smith",
                Email = "jane.smith@example.com",
                Phone = "098-765-4321",
                Role = RoleEnum.Customer
            };

            _users.Add(user1);
            _users.Add(user2);
        }

        #region Authentication

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public void Register(CreateUserDto createUserDto)
        {
            if (_users.FirstOrDefault(c => c.UserName == createUserDto.UserName) != null)
            {
                throw new BadHttpRequestException("Invalid UserName: UserName kept by someone else");
            }
            if (_users.FirstOrDefault(c => c.Email == createUserDto.Email) != null)
            {
                throw new BadHttpRequestException("Invalid Email: Email been kept");
            }
            var user = _mapper.Map<User>(createUserDto);
            user.Password = HashPassword(createUserDto.Password);
            user.ID = ++_lastId;
            user.TokenExpiry = null;
            _users.Add(user);
        }

        public string Login(LoginUserDto loginUserDto)
        {
            var user = _users.FirstOrDefault(c => c.UserName == loginUserDto.UserName);
            if (user == null)
            {
                throw new BadHttpRequestException("Invalid UserName: UserName not found");
            }
            var hashedPassword = HashPassword(loginUserDto.Password);
            if (user.Password != hashedPassword)
            {
                throw new BadHttpRequestException("Invalid Password: Password is incorrect");
            }
            user.Token = GenerateSecureToken();
            user.TokenExpiry = DateTime.UtcNow.AddMinutes(5);
            return user.Token;
        }

        private string GenerateSecureToken(int byteLength = 32)
        {
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(byteLength);
            return Convert.ToBase64String(tokenBytes)
                .Replace("/", "_")  // URL-safe
                .Replace("+", "-")
                .TrimEnd('=');
        }

        public bool Authorize(string token, RoleEnum[] roles){
            var user = _users.FirstOrDefault(c => c.Token == token);
            if(user == null)
                throw new UnauthorizedAccessException();
            if(roles.Contains(user.Role))
                return true;
            return false;
        }

        #endregion

        // TODO: Implement all the interface methods here for User!
        // We'll add them step by step as you learn
    }
} 