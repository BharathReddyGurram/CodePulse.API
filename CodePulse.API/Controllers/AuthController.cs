
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BCrypt.Net;

namespace CodePulse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                // Check if email exists
                if (await _userRepository.EmailExistsAsync(dto.Email))
                {
                    return BadRequest(new RegisterResponseDto
                    {
                        Success = false,
                        Message = "Email already exists"
                    });
                }

                // Hash password (use BCrypt or ASP.NET Identity in production)
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = passwordHash,
                    Role = UserRole.User
                };

                var createdUser = await _userRepository.CreateAsync(user);

                return Ok(new RegisterResponseDto
                {
                    Success = true,
                    Message = "User created successfully",
                    UserId = createdUser.Id.ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RegisterResponseDto
                {
                    Success = false,
                    Message = "Internal server error"
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid)
                return Unauthorized(new { message = "Invalid credentials" });

            // ====== IMPORTANT: explicit types here ======
            var keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var signingKey = new SymmetricSecurityKey(keyBytes);

            var claims = new List<System.Security.Claims.Claim>
    {
        new System.Security.Claims.Claim(
            System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,
            user.Id.ToString()
        ),
        new System.Security.Claims.Claim(
            System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email,
            user.Email ?? string.Empty
        ),
        new System.Security.Claims.Claim(
            System.Security.Claims.ClaimTypes.Role,
            user.Role.ToString()
        ),
        new System.Security.Claims.Claim(
            System.Security.Claims.ClaimTypes.Name,
            user.FirstName.ToString()
        )
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"],
                SigningCredentials = new SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new AuthResponseDto
            {
                Token = tokenString,
                ExpiresAt = tokenDescriptor.Expires!.Value,
                Role = user.Role.ToString(),
                Firstname = user.FirstName
            });
        }

    }
}
