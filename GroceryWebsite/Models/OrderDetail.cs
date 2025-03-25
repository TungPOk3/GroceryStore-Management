﻿using System.Text.Json.Serialization;

namespace GroceryWebsite.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
