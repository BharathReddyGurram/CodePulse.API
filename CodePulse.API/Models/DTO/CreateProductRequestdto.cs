namespace CodePulse.API.Models.DTO
{
    public class CreateProductRequestdto
    {

        public string Name { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }

        public string? Sku { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }

        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? CostPrice { get; set; }

        public int StockQuantity { get; set; } = 0;

        // primary category (optional)
        public int? CategoryId { get; set; }

        // 1+ categories from UI
        public List<int> CategoryIds { get; set; } = new();

        public string? Tags { get; set; }

        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? WidthCm { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? DepthCm { get; set; }

        public IFormFile? Image { get; set; }
    }
}
