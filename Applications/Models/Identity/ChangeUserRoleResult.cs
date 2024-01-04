using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Identity
{
    public record ChangeUserRoleResult
    {
        public bool IsSuccess { get; init; }
        public List<Error> Errors { get; init; }
    }
}
