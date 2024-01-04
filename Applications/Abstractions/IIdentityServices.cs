using Application.Models.Identity;
namespace Application.Abstractions
{
    public interface IIdentityServices
    {
        Task<AuthenticationResult> Registration(string email, string password, string phone, string profileName);
        Task<AuthenticationResult> Login(string email, string password);
        Task<AuthenticationResult> RefreshToken(string refreshToken);
        Task<AuthenticationResult> RevokeRefreshToken(string refreshToken);
        Task<ChangeUserRoleResult> AssignRole(string userId,string role);
        Task<ChangeUserRoleResult> RemoveRole(string userId,string role);
    }
}
