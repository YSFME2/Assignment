using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Responses
{
    public record ProductResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public decimal DiscountPercent { get; set; }


        public CategoryResponse Category { get; init; }
    }
}
