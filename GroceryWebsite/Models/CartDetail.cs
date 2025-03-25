using System.Text.Json.Serialization;

namespace GroceryWebsite.Models
{
    public class CartDetail
    {
        public int CartDetailId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Foreign Key
        public int CartId { get; set; }

        [JsonIgnore]
        public Cart Cart { get; set; }
        public int ProductId { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }
    }
}
