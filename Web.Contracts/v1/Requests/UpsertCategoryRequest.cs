using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Contracts.v1.Requests
{
    public record UpsertCategoryRequest
    {
        [MaxLength(250),MinLength(3),DisallowNull]
        public string Name { get; init; } = null!;
    }
}
