using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        [Length(2,150)]
        public string ProfileName { get; set; } = null!;

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
