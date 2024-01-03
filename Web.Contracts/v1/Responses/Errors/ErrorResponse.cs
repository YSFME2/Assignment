using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Responses.Errors
{
    public record ErrorResponse
    {
        public List<ErrorModel> Errors { get; init; }

        public static implicit operator ErrorResponse(string message) => new ErrorResponse() { Errors = new List<ErrorModel>() { new ErrorModel() { Message = message } } };
    }
}
