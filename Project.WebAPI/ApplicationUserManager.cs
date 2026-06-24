using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Project.Data.ExtendedDBEntities;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Options;
using Google;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Buffers.Text;
using System.Diagnostics;
using System.Text;
using System.Net;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Project.WebAPI
{
    //public class ApplicationUserManager : UserManager<DerivedIdentityUser>
    //{
    //    private readonly UserStore<IdentityUser, IdentityRole, ProjectDbContext, string, IdentityUserClaim<string>,
    //        IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>
    //        _store;
    //    private readonly  IConfiguration Configuration; 
    //    public ApplicationUserManager(
    //      IUserStore<DerivedIdentityUser> store,
    //      IOptions<IdentityOptions> optionsAccessor,
    //      IPasswordHasher<DerivedIdentityUser> passwordHasher,
    //      IEnumerable<IUserValidator<DerivedIdentityUser>> userValidators,
    //      IEnumerable<IPasswordValidator<DerivedIdentityUser>> passwordValidators,
    //      ILookupNormalizer keyNormalizer,
    //      IdentityErrorDescriber errors,
    //      IServiceProvider services,
    //         ILogger<UserManager<DerivedIdentityUser>> logger,IConfiguration configuration)
    //        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    //    {
    //        //_store = (UserStore<IdentityUser, IdentityRole, ProjectDbContext, string, IdentityUserClaim<string>,
    //        //IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>)store;
    //        Configuration = configuration;
    //    }

    //    private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //    private static readonly TimeSpan _timestep = TimeSpan.FromMinutes(3);
    //    private static readonly Encoding _encoding = new UTF8Encoding(false, true);

    //    private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
    //    {
    //        // # of 0's = length of pin
    //        const int mod = 1000000;

    //        // See https://tools.ietf.org/html/rfc4226
    //        // We can add an optional modifier
    //        var timestepAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
    //        var hash = hashAlgorithm.ComputeHash(ApplyModifier(timestepAsBytes, modifier));

    //        // Generate DT string
    //        var offset = hash[hash.Length - 1] & 0xf;
    //        Debug.Assert(offset + 4 < hash.Length);
    //        var binaryCode = (hash[offset] & 0x7f) << 24
    //                         | (hash[offset + 1] & 0xff) << 16
    //                         | (hash[offset + 2] & 0xff) << 8
    //                         | (hash[offset + 3] & 0xff);

    //        return binaryCode % mod;
    //    }

    //    public virtual async Task<bool> ValidateAsync(string purpose, string token, UserManager<DerivedIdentityUser> manager, DerivedIdentityUser user)
    //    {
    //        var key = await manager.GetAuthenticatorKeyAsync(user);
    //        int code;
    //        if (!int.TryParse(token, out code))
    //        {
    //            return false;
    //        }

    //        var hash = new HMACSHA1(Base32.FromBase32(key));
    //        var unixTimestamp = Convert.ToInt64(Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds));
    //        var timestep = Convert.ToInt64(unixTimestamp / 30);
    //        // Allow codes from 90s in each direction (we could make this configurable?)
    //        for (int i = -2; i <= 2; i++)
    //        {
    //            var expectedCode = Rfc6238AuthenticationService.ComputeTotp(hash, (ulong)(timestep + i), modifier: null);
    //            if (expectedCode == code)
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //}
}
