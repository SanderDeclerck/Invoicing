using IdentityServer4.Services;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Identity.Service.Domain;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Identity.Service.Data;
using System;

namespace Identity.Service.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ProfileService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var user = await _userManager.GetUserAsync(subject);
            await _dbContext.Entry(user).Reference(u => u.Tenant).LoadAsync();

            context.IssuedClaims = GetClaimsFromUser(user).ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var user = await _userManager.GetUserAsync(subject);

            context.IsActive = false;

            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                        if (db_security_stamp != security_stamp)
                            return;
                    }
                }

                context.IsActive =
                    !user.LockoutEnabled ||
                    !user.LockoutEnd.HasValue ||
                    user.LockoutEnd <= DateTime.Now;
            }
        }

        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            if (user.Tenant != null)
            {
                claims.AddRange(new[]
                {
                    new Claim("tenant_id", user.Tenant.Id),
                    new Claim("tenant_name", user.Tenant.Name)
                });
            }

            if (_userManager.SupportsUserEmail)
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }

            if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }

            return claims;
        }
    }
}