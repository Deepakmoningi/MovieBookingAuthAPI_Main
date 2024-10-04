using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MovieBookingAuthApi.Interfaces;
using MovieBookingAuthApi.Models;


namespace MovieBookingAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MongoDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(MongoDbContext context, ITokenService tokenService, ILogger<LoginController> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] Register registerObj)
        {
            var existingEmail = await _context.users.Find(x=>x.Email== registerObj.Email).FirstOrDefaultAsync();

            var existingLoginId = await _context.users.Find(x=>x.LoginId==registerObj.LoginId).FirstOrDefaultAsync();

            if (existingLoginId != null)
            {
                return BadRequest(new
                {
                    message = $"{registerObj.LoginId} LoginId already exists"
                });
            }

            if(existingEmail != null)
            {
                return BadRequest(new
                {
                    message = $"{registerObj.Email} EmailId already exists"
                });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerObj.Password);

            var user = new User
            {
                FirstName = registerObj.FirstName,
                LastName = registerObj.LastName,
                Email = registerObj.Email,
                LoginId = registerObj.LoginId,
                Role = registerObj.Role,
                ContactNumber = registerObj.ContactNumber,
                Password = hashedPassword
            };

            _logger.LogInformation($"Registering a new User");

            await _context.users.InsertOneAsync(user);
            return Ok(new
            {
                message = "Registration Successfull"
            });
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login loginObj)
        {
            _logger.LogInformation("Validating LoginId and password of user for Login");

            var user = await _context.users.Find(x=>x.LoginId== loginObj.LoginId).FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest(new
                {
                    message = "Invalid LoginId/Password"
                });
            }

            //string salt = BCrypt.Net.BCrypt.GenerateSalt();

            //string hashedPassword = BCrypt.Net.BCrypt.HashPassword("yourPassword", salt);

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginObj.Password, user.Password);

            if (!isPasswordValid)
            {
                return BadRequest(new
                {
                    message = "Invalid LoginId or Password"
                });
            }


            _logger.LogInformation($"{loginObj.LoginId} logged in");
            _logger.LogInformation("Generating token after succesful login");

            var token = _tokenService.CreateToken(user);

            return Ok(new { loginId = user.LoginId, jwtToken = token });

        }

        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword requestObj)
        {

            var user = await _context.users.Find(x => x.LoginId == requestObj.LoginId).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest(new
                {
                    message = $"{requestObj.LoginId} User doesn't exists"
                });
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(requestObj.NewPassword);
            // var updateDefinition = Builders<User>.Update.Set(x => x.Password, requestObj.NewPassword);
            var updateDefinition = Builders<User>.Update.Set(x => x.Password, hashedPassword);
            await _context.users.UpdateOneAsync(x=>x.LoginId == requestObj.LoginId, updateDefinition); 
            return Ok(new
            {
                message = "Password updated successfully"
            });
        }

    }
}
