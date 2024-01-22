using System.Security.Cryptography;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;
using OutsourcePlatformApp.Service.EmailServices;
using OutsourcePlatformApp.Utils;

namespace OutsourcePlatformApp.Service
{
    public class AuthService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtTokenService jwtTokenService;
        private readonly IEmailSenderService emailSenderService;
        private readonly IActivationTokenRepository activationTokenRepository;

        public AuthService(IUserRepository userRepository, JwtTokenService jwtTokenService,
            IEmailSenderService emailSenderService, IActivationTokenRepository activationTokenRepository)
        {
            this.userRepository = userRepository;
            this.jwtTokenService = jwtTokenService;
            this.emailSenderService = emailSenderService;
            this.activationTokenRepository = activationTokenRepository;
        }

        public async Task RegisterExecutorAsync(ExecutorRegisterDto registerDto)
        {
            try
            {
                var executor = ExecutorRegisterDto.ConvertToExecutor(registerDto);
                var activationToken = Guid.NewGuid();
                executor.User.ActivationTokens = new List<ActivationToken>
                {
                    new ActivationToken
                    {
                        Token = activationToken
                    }
                };
                executor.User.IsVerified = true;
                await userRepository.CreateExecutorAsync(executor);
                //await emailSenderService.SendEmail(executor.User.Email, activationToken.ToString());
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
                var activationToken = Guid.NewGuid();
                customer.User.ActivationTokens = new List<ActivationToken>
                {
                    new ActivationToken
                    {
                        Token = activationToken
                    }
                };
                await userRepository.CreateCustomerAsync(customer);
              //  await emailSenderService.SendEmail(customer.User.Email, activationToken.ToString());
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
                Username = user.Username,
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

        public async Task<AuthResponseDto> VerifyAccount(Guid code)
        {
            var user = await userRepository.GetUserbyActivationCode(code);
            ;
            if (user == null)
                throw new ArgumentException("Пользователь не найден");
            var userVerificationCodes = await activationTokenRepository.GetActivationTokensByUserId(user.Id);
            var currentCode = userVerificationCodes.FirstOrDefault(x => x.Token == code);
            if (currentCode == null)
                throw new ArgumentException("Токен не валидный");
            //if (currentCode.ExpiresAt >= DateTimeOffset.UtcNow || currentCode.IsActivated)
            //    throw new ArgumentException("Срок токена истек");
            user.IsVerified = true;
            currentCode.IsActivated = true;
            await userRepository.UpdateUserAsync(user);
            await activationTokenRepository.UpdateTokenAsync(currentCode);
            return new AuthResponseDto
            {
                Token = jwtTokenService.GenerateToken(user),
                Username = user.Username,
                Role = user.UserRoles.First().Name,
                RefreshToken = user.RefreshToken.Token
            };
        }
    }
}