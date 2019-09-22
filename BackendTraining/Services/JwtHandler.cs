using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml;

namespace BackendTraining.Services
{
    public class JwtHandler
    {
        private readonly JwtSettings _settings;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private SecurityKey _issuerSigningKey;
        private SigningCredentials _signingCredentials;
        private JwtHeader _jwtHeader;
        public TokenValidationParameters Parameters { get; private set; }

        public JwtHandler(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
            InitializeRsa();
            InitializeJwtParameters();
        }

        private void InitializeRsa()
        {
            RSA publicRsa = RSA.Create();
            
            var publicKeyXml = File.ReadAllText(_settings.PublicKeyXML);
            publicRsa.RsaFromXmlString(publicKeyXml);
            _issuerSigningKey = new RsaSecurityKey(publicRsa);
            
            if (string.IsNullOrWhiteSpace(_settings.PrivateKeyXML))
            {
                return;
            }
            
            RSA privateRsa = RSA.Create();
            
            var privateKeyXml = File.ReadAllText(_settings.PrivateKeyXML);
            privateRsa.RsaFromXmlString(privateKeyXml);
            var privateKey = new RsaSecurityKey(privateRsa);
            _signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
        }

        private void InitializeJwtParameters()
        {
            _jwtHeader = new JwtHeader(_signingCredentials);
            Parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = _settings.Issuer,
                IssuerSigningKey = _issuerSigningKey
            };
        }

        public JWT Create(string userId)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(5);
            var centuryBegin = new DateTime(1970, 1, 1);
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = DateTime.Now;
            var issuer = _settings.Issuer ?? string.Empty;


            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(userId, "Login"),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, userId)
                }
            );

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Subject = identity,
                SigningCredentials = _signingCredentials,
                NotBefore = nowUtc,
                Expires = expires
            });
            var token = handler.WriteToken(securityToken);

            return new JWT
            {
                Token = token,
                Expires = exp
            };
        }
    }

    static class JwtExtensions
    {
        public static void RsaFromXmlString(this RSA rsa, string xmlString)
        {
            var parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }
    }
}
