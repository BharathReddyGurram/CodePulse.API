namespace CodePulse.API.Models.Domain
{
    public class Category
    {
        public int  Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
