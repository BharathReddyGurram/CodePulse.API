namespace CodePulse.API.Models.Domain
{
    public class Product
    {

            public int Id { get; set; }

            // Basic identity
            public string Name { get; set; } = string.Empty;
            public string? ShortDescription { get; set; }
            public string? Description { get; set; }

            // Catalog codes
            public string? Sku { get; set; }          // internal code
            public string? Barcode { get; set; }      // EAN/UPC if needed
            public string? Brand { get; set; }

            // Pricing
            public decimal Price { get; set; }        // main price
            public decimal? OldPrice { get; set; }    // before discount
            public decimal? CostPrice { get; set; }   // optional internal cost

            // Inventory
            public int StockQuantity { get; set; } = 0;
            public bool IsActive { get; set; } = true;
            public bool IsFeatured { get; set; } = false;

            // Classification
            public int CategoryId { get; set; }       // FK to Category
            public Category? Category { get; set; }

            public string? Tags { get; set; }         // e.g. "men, shoes, running"

            // Media
            public string? ThumbnailImageUrl { get; set; }   // small image
            public string? ImageUrl1 { get; set; }          // main
            public string? ImageUrl2 { get; set; }          // optional
            public string? ImageUrl4 { get; set; }          // optional

            // Physical details (for clothes, shoes, tools, electronics)
            public string? Color { get; set; }
            public string? Size { get; set; }               // "M", "42", etc.
            public decimal? WeightKg { get; set; }          // shipping weight
            public decimal? WidthCm { get; set; }
            public decimal? HeightCm { get; set; }
            public decimal? DepthCm { get; set; }

            // Meta
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime? UpdatedAt { get; set; }
        // Product.cs (add at bottom)
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();


    }
}
