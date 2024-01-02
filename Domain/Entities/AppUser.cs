using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public Roles Role { get; set; }
    }
}
