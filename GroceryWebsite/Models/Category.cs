using System.Text.Json.Serialization;

namespace GroceryWebsite.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
