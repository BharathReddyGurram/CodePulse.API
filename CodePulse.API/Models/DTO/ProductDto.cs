namespace CodePulse.API.Models.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? CostPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Tags { get; set; }
        public string? ThumbnailImageUrl { get; set; }
        public string? ImageUrl1 { get; set; }
        public string? ImageUrl2 { get; set; }
        public string? ImageUrl4 { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? WidthCm { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? DepthCm { get; set; }
    }
}
