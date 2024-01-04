using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Requests
{
    public record UpsertProductRequest
    {
        [MaxLength(250)]
        public string Name { get; init; } = null!;
        [MaxLength(1000)]
        public string? Description { get; init; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; init; }
        [Range(0, 1)]
        public decimal DiscountPercent { get; set; }

        public int CategoryId { get; init; }
    }
}
