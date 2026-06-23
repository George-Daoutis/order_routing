using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using order_routing.Server.Data;
using order_routing.Server.Models;
using order_routing.Server.Services;


namespace order_routing.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly OrderDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public AuthController(OrderDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDto registerDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Username.ToLower() == registerDto.Username.ToLower()))
            {
                return BadRequest("Το όνομα χρήστη χρησιμοποιείται ήδη.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = passwordHash,
                Role = registerDto.Role,
                StoreId = registerDto.StoreId
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Ο χρήστης δημιουργήθηκε επιτυχώς!");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == loginDto.Username.ToLower());

            if (user == null)
            {
                return Unauthorized("Λάθος όνομα χρήστη.");
            }


            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized("Λάθος κωδικός.");
            }



            var token = _tokenService.CreateToken(user);

            return new UserDto
            {
                Username = user.Username,
                Token = token,
                Role = user.Role,
                StoreId = user.StoreId
            };
        }
    }
}