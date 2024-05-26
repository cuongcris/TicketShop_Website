using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TicketOnline.Data;
using TicketOnline.Model;

namespace DOC_SYS.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _context;
        public AuthenController(IConfiguration configuration, ApplicationContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<CustomerDTO>> Register(CustomerDTO request)
        {
            string err = string.Empty;

            var checkEmail = await _context.Customers.AnyAsync(x => x.Email == request.Email);
            var checkPhone = await _context.Customers.AnyAsync(x => x.PhoneNumber == request.PhoneNumber);

            if (checkEmail) err += "Existed email\n";
            if (checkPhone) err += "Existed phone\n";

            if (err != string.Empty)
                return BadRequest(err);

            Customer user = new Customer();


            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Email = request.Email;
            user.Name = request.FullName;
            user.Dob = DateOnly.Parse(request.Dob);
            user.PhoneNumber = request.PhoneNumber;
            _context.Customers.Add(user);
            await _context.SaveChangesAsync();
            return StatusCode(201, user);
        }

        [HttpGet("token")]
        [Authorize()]
        public async Task<ActionResult<string>> GetToken()
        {
            var token = HttpContext.Request.Cookies["Bearer"];
            //If token is null, generate it and return
            if (token == null) return BadRequest("Token is null");

            //Return token
            return Ok(
                    new LoginResponse(
                        await _context.Customers.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name)),
                        token
                        )
                );
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(CustomerLogin request)
        {
            Customer user = new Customer();
            user = await _context.Customers.FirstOrDefaultAsync(u => u.Email.Equals(request.Email));
            if (user == null)
                return BadRequest("Email not existed!");
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password!");
            }
            string token = CreateToken(user);
            CookieOptions cookie = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1),
                Secure = true,
                SameSite = SameSiteMode.None,
            };

            CookieOptions userIdCookie = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1),
                Secure = true,
                SameSite = SameSiteMode.None,
            };
            HttpContext.Response.Cookies.Append("Bearer", token, cookie);
            HttpContext.Response.Cookies.Append("customerid", user.Id, userIdCookie);

            return Ok(new LoginResponse(
                    user, token
                    ));
        }

        private string CreateToken(Customer user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                user.Email == "admin@gmail.com" ? new Claim(ClaimTypes.Role, "Admin") : new Claim(ClaimTypes.Role, "Customer"),
                new Claim("customerid", user.Id)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
