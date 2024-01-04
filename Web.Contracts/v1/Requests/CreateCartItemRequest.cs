using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Requests
{
    public record CreateCartItemRequest
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; init; }
        public int ProductId { get; set; }
    }
}
