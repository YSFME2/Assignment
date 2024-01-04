using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Requests
{
    public record CategoryRequest
    {
        [Length(3, 150)]
        public string Name { get; init; } = null!;
        [MaxLength(500)]
        public string? Description { get; init; }
    }
}
