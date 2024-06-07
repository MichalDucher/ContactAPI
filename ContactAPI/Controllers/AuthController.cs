using ContactAPI.Data;
using ContactAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContactAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //Sprawdza poprawność danych logowania i zwraca token potrzebny do autentykacji
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var _user = _context.Users.FirstOrDefault(x => x.username == user.username && x.password == HashPassword(user.password));
            if (_user != null)
            {
                var token = GenerateToken(_user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Wrong login or password :/");
        }

        //Generuje token
        private string GenerateToken(User user)
        {
            // Pobierz konfigurację JWT z ustawień
            var jwtConf = _configuration.GetSection("Jwt");

            // Klucz do podpisywania tokenu
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Tworzenie listy roszczeń (claims)
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Name, user.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unikalny identyfikator tokenu
            };

            // Tworzenie nowego Tokenu
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Funkcja hashująca zwracająca hash podanego przez użytkownika hasła do porównania z tym w bazie danych
        private string HashPassword(string password)
        {
            // Tworzenie instancji obiektu SHA256
            using (var sha256 = SHA256.Create())
            {
                // Obliczanie hasha dla hasła
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Konwersja bajtów na ciąg znaków w formacie szesnastkowym
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                // Zwracanie zahashowanego hasła
                return builder.ToString();
            }
        }
    }
}
