using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Project.WebAPI.APIResource;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
//using Project.Data.Migrations;
using Project.Data.ExtendedDBEntities;
using Project.Services.IService;
using Project.Services.Service;
using Project.Models.User;
using Microsoft.Extensions.DependencyInjection;

namespace Project.WebAPI.Infrastructure
{
    public interface IJwtAuthManager
    {
        IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }
        JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now);
        JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);
        void RemoveExpiredRefreshTokens(DateTime now);
        void RemoveRefreshTokenByUserName(string userName);
        (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
    }

    public class JwtAuthManager : IJwtAuthManager
    {
        public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
        private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;  //  can store in a database or a distributed cache
        private readonly JwtTokenConfig _jwtTokenConfig;
        private readonly byte[] _secret;
        private readonly IServiceProvider _serviceProvider;

        public JwtAuthManager(JwtTokenConfig jwtTokenConfig, IServiceProvider serviceProvider)
        {
            _jwtTokenConfig = jwtTokenConfig;
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
            _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
            _serviceProvider = serviceProvider;
        }

        //  optional: clean up expired refresh tokens
        public void RemoveExpiredRefreshTokens(DateTime now)
        {
            var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();
            foreach (var expiredToken in expiredTokens)
            {
                _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
            }
        }

        //  can be more specific to ip, user agent, device name, etc.
        public void RemoveRefreshTokenByUserName(string userName)
        {
            var refreshTokens = this._usersRefreshTokens.Where(x => x.Value.UserName == userName).ToList();
            foreach (var refreshToken in refreshTokens)
            {
                this._usersRefreshTokens.TryRemove(refreshToken.Key, out _);
            }
        }

        public JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
        {
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

  

            var jwtToken = new JwtSecurityToken(
                _jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshToken
            {
                UserName = username,
                TokenString = GenerateRefreshTokenString(),
                ExpireAt = now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
            };
            _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);

            // Persist durably so refresh survives across requests: this class is registered
            // Transient (see Startup.cs), so _usersRefreshTokens above is only ever visible to
            // THIS instance -- a different request resolves a brand-new, empty dictionary.
            // Refresh() below already has a DB-backed fallback (DerivedIdentityUser.RefreshToken
            // / RefreshTokenExpiryTime), it was just never populated because this write used to
            // be commented out (and referenced an IAccountService.UpdateUserToken method that
            // was never actually implemented). Best-effort: a persistence failure here must
            // never break login/token issuance itself -- the in-memory dictionary still lets a
            // same-instance refresh succeed as before.
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DerivedIdentityUser>>();
                var user = userManager.FindByNameAsync(username).Result;
                if (user != null)
                {
                    user.RefreshToken = refreshToken.TokenString;
                    user.RefreshTokenExpiryTime = refreshToken.ExpireAt;
                    userManager.UpdateAsync(user).Wait();
                }
            }
            catch (Exception)
            {
                // Best-effort only -- see comment above.
            }

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserName = username,
            };
        }

        public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
        {
            var (principal, jwtToken) = this.DecodeJwtToken(accessToken);
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException(APIResources.InvalidToken);
            }

            var userName = principal.Identity.Name;

            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DerivedIdentityUser>>();
            var user = userManager.FindByNameAsync(userName).Result;

            if (!this._usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
            {
                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    throw new SecurityTokenException(APIResources.InvalidToken);
                }
            }
            else if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < now)
            {
                throw new SecurityTokenException(APIResources.InvalidToken);
            }

            //if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < now)
            //{
            //    throw new SecurityTokenException(APIResources.InvalidToken);
            //}

            return this.GenerateTokens(userName, principal.Claims.ToArray(), now); //  need to recover the original claims
        }

        public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException(APIResources.InvalidToken);
            }
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _jwtTokenConfig.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_secret),
                        ValidAudience = _jwtTokenConfig.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    },
                    out var validatedToken);
            return (principal, validatedToken as JwtSecurityToken);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    } 
}
