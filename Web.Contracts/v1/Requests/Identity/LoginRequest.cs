using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Requests.Identity
{
    public record LoginRequest
    {
        [Length(10,150),EmailAddress]
        public string Email { get; init; }
        [Length(8,150)]
        public string Password { get; init; }
    }
}
