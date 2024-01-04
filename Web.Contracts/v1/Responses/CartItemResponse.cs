using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Responses
{
    public record CartItemResponse
    {
        public int Id { get; set; }
        public int Quantity { get; init; }
        public ProductResponse Product { get; init; }
    }
}
