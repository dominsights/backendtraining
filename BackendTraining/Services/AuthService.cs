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
        private JwtHandler _jwtHandler;
        private List<User> _users;

        public UserJwt LoginAsync(string userName, string password)
        {
            try
            {
                var userIdentity = _users.First(u => u.UserName.Equals(userName) && u.Password.Equals(password));
                var jwt = _jwtHandler.Create(userIdentity.Id.ToString());
                var payload = new UserJwt() { Id = userIdentity.Id, Token = jwt.Token, ExpiresIn = jwt.Expires };

                return payload;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public AuthService(JwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;

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
