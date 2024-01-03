using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Responses.Errors
{
    public record ErrorModel
    {
        public string Field { get; init; }
        public string Message { get; init; }
    }
}
