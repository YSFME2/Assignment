using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Contracts.v1.Responses.Errors;

namespace Web.Contracts.v1.Responses.Identity
{
    public record ChangeUserRoleResponse(bool IsSuccess, List<ErrorResponse> Errors);
}
