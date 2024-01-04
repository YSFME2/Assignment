using Application.Models.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web.Contracts.v1.Responses.Errors;
using Microsoft.EntityFrameworkCore;
using Application.Models;
using Microsoft.Extensions.Options;

namespace Web.Api.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityServices(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthenticationResult> RegisterAsync(string profileName, string email, string password, string phone)
        {
            var x = _jwtSettings.Secret;
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is not null)
            {
                return new AuthenticationResult
                {
                    Errors = [new("Register", "Email is already exist")]
                };
            }

            var appUser = new AppUser
            {
                Email = email,
                UserName = email,
                ProfileName = profileName,
            };

            var result = await _userManager.CreateAsync(appUser, password.Trim());
            if (!result.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = result.Errors.Select(error => new Error(error.Code, error.Description)).ToList(),
                };
            }

            await _userManager.AddToRoleAsync(appUser, "User");

            return await GenerateToken(appUser);
        }

        public async Task<AuthenticationResult> RegisterAsAdminAsync(string profileName, string email, string password, string phone)
        {
            var x = _jwtSettings.Secret;
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is not null)
            {
                return new AuthenticationResult
                {
                    Errors = [new("", "Email is already exist")]
                };
            }

            var appUser = new AppUser
            {
                Email = email,
                UserName = email,
                ProfileName = profileName,
            };

            var result = await _userManager.CreateAsync(appUser, password.Trim());
            if (!result.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = result.Errors.Select(error => new Error(error.Code, error.Description)).ToList(),
                };
            }

            await _userManager.AddToRoleAsync(appUser, "Admin");

            return await GenerateToken(appUser);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return new AuthenticationResult
                {
                    Errors = [new("UserCredentials", "User not Exist or wrong credentials")]
                };
            }
            return await GenerateToken(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
            {
                var result = new AuthenticationResult() { Errors = new List<Error>() };
                result.Errors.Add(new Error("RefreshTokenAsync", "Invalid or Expired Refresh Token"));
                return result;
            }

            var token = user.RefreshTokens.First(x => x.Token == refreshToken);
            if (!token.IsActive)
            {
                var result = new AuthenticationResult() { Errors = new List<Error>() };
                result.Errors.Add(new Error("RefreshTokenAsync", "Invalid or Expired Refresh Token"));
                return result;
            }

            token.RevokedOn = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return await GenerateToken(user);
        }

        public async Task<AuthenticationResult> RevokeRefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
                return new() { Errors = [new("RevokeRefreshTokenAsync", "Invalid refresh token")] };


            var token = user.RefreshTokens.First(x => x.Token == refreshToken);
            if (!token.IsActive)
                return new() { IsSuccess = true };

            token.RevokedOn = DateTime.Now;
            await _userManager.UpdateAsync(user);
            return new() { IsSuccess = true };
        }

        public async Task<ChangeUserRoleResult> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _roleManager.RoleExistsAsync(role))
                return new() { Errors = [new("ChangeUserRole", "User Id or Role is invalid!")] };

            await _userManager.AddToRoleAsync(user, role);
            return new ChangeUserRoleResult { IsSuccess = true };
        }

        public async Task<ChangeUserRoleResult> RemoveRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _roleManager.RoleExistsAsync(role) || !await _userManager.IsInRoleAsync(user, role))
                return new() { Errors = [new("ChangeUserRole", "User Id or Role is invalid!")] };

            await _userManager.RemoveFromRoleAsync(user, role);
            return new ChangeUserRoleResult { IsSuccess = true };
        }

        private async Task<AuthenticationResult> GenerateToken(AppUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                claims.Add(new Claim("roles", role));

            claims.Add(new(JwtRegisteredClaimNames.Sub, user.UserName));
            claims.Add(new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new(JwtRegisteredClaimNames.Name, user.ProfileName));
            claims.Add(new("uid", user.Id));

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.Now.AddDays(_jwtSettings.ExpirationInHours);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials);

            var authenticationResult = new AuthenticationResult
            {
                IsSuccess = true,
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

            if (user.RefreshTokens.Any(x => x.IsActive))
            {
                var refreshToken = user.RefreshTokens.First(x => x.IsActive);
                refreshToken.ExpireOn = DateTime.Now.AddDays(_jwtSettings.RefreshExpirationInDays);
                await _userManager.UpdateAsync(user);
                authenticationResult.RefreshToken = refreshToken.Token;
                authenticationResult.RefreshTokenExpiration = refreshToken.ExpireOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken(user.Id);
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
                authenticationResult.RefreshToken = refreshToken.Token;
                authenticationResult.RefreshTokenExpiration = refreshToken.ExpireOn;
            }

            return authenticationResult;
        }

        private RefreshToken GenerateRefreshToken(string userId)
        {
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(Encoding.ASCII.GetBytes(userId += DateTime.UtcNow.Ticks.ToString() + Guid.NewGuid().ToString())),
                CreatedOn = DateTime.Now,
                ExpireOn = DateTime.Now.AddDays(_jwtSettings.RefreshExpirationInDays),
            };
        }
    }
}
