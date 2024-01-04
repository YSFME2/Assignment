using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Web.Contracts.v1.Requests.Identity
{
    public record RegisterRequest : LoginRequest
    {
        [Length(3, 60)]
        public string ProfileName { get; init; }
        [Length(7, 15), Phone]
        public string Phone { get; init; }
    }
}
