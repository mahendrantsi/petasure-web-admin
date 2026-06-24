using Project.WebAPI.Auth;
using Project.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.WebAPI.Helpers
{
    public class Tokens
    {
        public static async Task<dynamic> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            // var response = new
            return new
            {
                // id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),

                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
            };

            //  return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
