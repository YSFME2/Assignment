using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Requests.Identity
{
    public record ChangeUserRoleRequest(string UserId,string Role);
}
