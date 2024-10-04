using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MovieBookingAuthApi.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieBookingAuthApi.Models
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly MongoDbContext _context;

        public TokenService(IConfiguration config, MongoDbContext context)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _context = context;
        }
        public string CreateToken(User userObj)
        {
            //var claims = new List<Claim>
            //{
            //    new Claim(JwtRegisteredClaimNames.UniqueName,userObj.LoginId),
            //    new Claim(JwtRegisteredClaimNames.Email,userObj.Email)
            //};

        
            //var user = _context.users.Find(x => x.LoginId == userObj.LoginId).FirstOrDefaultAsync();
            

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                  Subject = new ClaimsIdentity(new Claim[]
                   {
                    new Claim(ClaimTypes.Name, userObj.LoginId),
                    new Claim(ClaimTypes.Email, userObj.Email),
                    new Claim(ClaimTypes.Role, userObj.Role) // Add roles here
                   }),

                //Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                //Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = creds,
                Issuer = "Deepak",
                Audience = "Deepak"
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
