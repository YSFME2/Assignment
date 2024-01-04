
namespace Web.Contracts.v1.Requests
{
    public record ProductFilterRequest 
    {
        public string? FilterText { get; set; }

        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }

        public int? CategoryId { get; set; }

    }
}
