using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;
using OutsourcePlatformApp.Utils;

namespace OutsourcePlatformApp.Service
{
    public class AuthService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtTokenService jwtTokenService;

        public AuthService(IUserRepository userRepository, JwtTokenService jwtTokenService)
        {
            this.userRepository = userRepository;
            this.jwtTokenService = jwtTokenService;
        }

        public async Task RegisterExecutorAsync(ExecutorRegisterDto registerDto)
        {
            try
            {
                var executor = ExecutorRegisterDto.ConvertToExecutor(registerDto);
                await userRepository.CreateExecutorAsync(executor);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task RegisterCustomerAsync(CustomerRegisterDto registerDto)
        {
            try
            {
                if (!InnCheck.CheckInn(registerDto.Inn))
                    throw new ArgumentException("Неправильный ИНН");
                var customer = CustomerRegisterDto.ConvertToCustomer(registerDto);
                await userRepository.CreateCustomerAsync(customer);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<AuthResponseDto> Authenticate(AuthRequestDto authRequestDto)
        {
            var user = await userRepository.GetUserByUsernameAsync(authRequestDto.UserName);
            if (!BCrypt.Net.BCrypt.Verify(authRequestDto.Password, user.Password))
                throw new ArgumentException("Неверный логин/пароль");

            if (user.RefreshToken.Expires == DateOnly.FromDateTime(DateTime.Now))
            {

                user.RefreshToken.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                user.RefreshToken.Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6));
                user.RefreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                await userRepository.UpdateUserAsync(user);
            }

            var response = new AuthResponseDto
            {
                Token = jwtTokenService.GenerateToken(user),
                Role = user.UserRoles.First().Name,
                RefreshToken = user.RefreshToken.Token
            };
            return response;
        }

        public async Task<string> UpdateToken(string refreshToken)
        {
            var user = await userRepository.GetUserByRefreshToken(refreshToken);
            if (user.RefreshToken.Expires == DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Устаревшие данные");
            return jwtTokenService.GenerateToken(user);
        }

        
    }
}