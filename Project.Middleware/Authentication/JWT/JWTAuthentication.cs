using Project.Core.Extension;
using Project.Models.CommonModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Project.Middleware.Authentication.JWT
{
    public class JWTAuthentication
    {
        private IConfiguration _config;
        // / <summary>
        // / Initializes a new instance of the <see cref="JWTAuthentication"/> class.
        // / </summary>
        // / <param name="config"></param>
        public JWTAuthentication(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            IEnumerable<Claim> Claims = new Claim[] {
                                        new Claim("id", userInfo.Id),
                                        new Claim("userName",userInfo.Username),
                                        new Claim("dateOfCreat",DateTime.UtcNow.ToString()),
                                         new Claim("ipAddress",CommonExtension.GetRequestIP())
                                      };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              Claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            string tt= new JwtSecurityTokenHandler().WriteToken(token);
            bool tttt = ValidateToken(tt);
            return tt;
        }

        private bool ValidateToken(string authToken)
        {
            bool valid = true;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = false, //  Because there is no expiration in the generated token
                    ValidateAudience = false, //  Because there is no audiance in the generated token
                    ValidateIssuer = false,   //  Because there is no issuer in the generated token
                    ValidIssuer = "Sample",
                    ValidAudience = "Sample",
                    IssuerSigningKey = securityKey //  The same key as the one that generate the token
                };
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                var claims = principal.Claims.ToList();
                string id = claims[0].Value.ToString();
                string userName = claims[1].Value.ToString();
                string ipAddress = claims[3].Value.ToString();
            }
            catch { valid = false; }
            return valid;
        }

        
    }
}
