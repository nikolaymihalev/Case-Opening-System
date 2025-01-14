using CaseOpener.API.Extensions;
using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Enums;
using CaseOpener.Core.Models.Transaction;
using CaseOpener.Core.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaseOpener.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        private readonly IAdminService adminService;
        private readonly ITransactionService transactionService;
        private readonly ICaseService caseService;
        private readonly JwtSettings jwtSettings;

        public UserController(
            IUserService _userService,
            IConfiguration _configuration,
            IAdminService _adminService,
            ITransactionService _transactionService,
            ICaseService _caseService,
            IOptions<JwtSettings> _jwtSettings)
        {
            userService = _userService;
            configuration = _configuration;
            adminService = _adminService;
            transactionService = _transactionService;
            caseService = _caseService;
            jwtSettings = _jwtSettings.Value;

        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "No token provided" });
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.Key);

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var email = principal.FindFirst(ClaimTypes.Name)?.Value;

                if (email is null)
                    return BadRequest(new { Message = "Email is not valid!" });

                var userInfo = await userService.GetUserByEmailAsync(email);

                if (userInfo is null)
                    throw new Exception();

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInformation(string userId)
        {
            try
            {
                var user = await userService.GetUserAsync(userId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("is-admin")]
        public async Task<IActionResult> IsUserAdmin(string userId)
        {
            var result = await adminService.CheckUserIsAdmin(userId);

            return Ok(new { Message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) 
        {
            try
            {
                string operation = await userService.LoginAsync(model);

                if (operation == ReturnMessages.SuccessfullyLoggedIn)
                {
                    string userRole = await userService.GetUserRoleAsync(model.Email);

                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Role, userRole)
                };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var keyBytes = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddHours(24),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(keyBytes),
                            SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { token = tokenString });
                }

                return BadRequest(new { Message = operation });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var user = await userService.RegisterAsync(model);

                await adminService.AddUserToRoleAsync(user.Id, "User");
                await caseService.SubscribeUserToDailyRewardAsync(user.Id);

                var transaction = new TransactionModel()
                {
                    UserId = user.Id,
                    Type = TransactionType.Deposit.ToString(),
                    Amount = 500m,
                    Date = DateTime.UtcNow,
                    Status = TransactionStatus.Completed.ToString()
                };

                await transactionService.AddTransactionAsync(transaction);

                return Ok(new { Message = ReturnMessages.SuccessfullyRegistered });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-info")]
        public async Task<IActionResult> Update(string userId, string username)
        {
            try
            {
                string operation = await userService.UpdateUserInformationAsync(userId, username);

                return Ok(new { Message = operation });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-balance")]
        public async Task<IActionResult> UpdateBalance(string userId, string operation, decimal amount) 
        {
            try
            {
                string result = await userService.UpdateUserBalanceAsync(userId, operation, amount);

                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
