using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BackendTraining.Services
{
    public class AuthService
    {
        private List<User> _users;

        public async Task<UserPayload> LoginAsync(string userName, string password)
        {
            string privateKey = await ReadStringFromFileAsync(@"private.key");
            string publicKey = await ReadStringFromFileAsync(@"public.key");

            var privateKeyBytes = Encoding.ASCII.GetBytes(privateKey);

            try
            {
                var userIdentity = _users.First(u => u.UserName.Equals(userName) && u.Password.Equals(password));

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userIdentity.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKeyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                string tokenText = jwtTokenHandler.WriteToken(token);
                long expiresIn = ((DateTimeOffset)tokenDescriptor.Expires.Value).ToUnixTimeSeconds();

                var payload = new UserPayload() { Id = userIdentity.Id, Token = tokenText, ExpiresIn = expiresIn };

                return payload;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public AuthService()
        {
            _users = new List<User>()
            {
                new User() {Id = 1, UserName = "domicio", Password = "password" },
                new User() {Id = 1, UserName = "test", Password = "test" }
            };
        }

        private async Task<string> ReadStringFromFileAsync(string path)
        {
            if(File.Exists(path))
            {
                return await File.ReadAllTextAsync(path);
            }
            else
            {
                throw new ArgumentException("You need to provide your own .key files.");
            }
        }
    }
}
