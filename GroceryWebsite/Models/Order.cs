using System.Text.Json.Serialization;

namespace GroceryWebsite.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }
        public string ShippingMethod { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }

}
