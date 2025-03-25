using System.Text.Json.Serialization;

namespace GroceryWebsite.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; } = string.Empty;

        // Foreign Key
        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        [JsonIgnore]
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}
