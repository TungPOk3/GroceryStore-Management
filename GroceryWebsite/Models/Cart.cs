using System.Text.Json.Serialization;

namespace GroceryWebsite.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Foreign Key
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}
