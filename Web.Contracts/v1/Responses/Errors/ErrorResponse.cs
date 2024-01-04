using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Responses.Errors
{
    public record ErrorResponse(string Code, string Description)
    {
        public static implicit operator ErrorResponse(string description) => new ErrorResponse("Error", description);

    }
}
