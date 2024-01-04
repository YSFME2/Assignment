using Application.Models.Identity;
namespace Application.Abstractions
{
    public interface IIdentityServices
    {
        Task<AuthenticationResult> RegisterAsync(string profileName, string email, string password, string phone);
        Task<AuthenticationResult> RegisterAsAdminAsync(string profileName, string email, string password, string phone);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
        Task<AuthenticationResult> RevokeRefreshTokenAsync(string refreshToken);
        Task<ChangeUserRoleResult> AssignRoleAsync(string userId, string role);
        Task<ChangeUserRoleResult> RemoveRoleAsync(string userId, string role);
    }
}
