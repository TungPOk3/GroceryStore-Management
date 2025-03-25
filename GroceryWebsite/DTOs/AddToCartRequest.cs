namespace GroceryWebsite.DTOs
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
