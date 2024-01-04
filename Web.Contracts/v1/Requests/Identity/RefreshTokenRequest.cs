using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Requests.Identity
{
    public record RefreshTokenRequest
    {
        [MinLength(100)]
        public string? RefreshToken { get; init; }
    }
}
