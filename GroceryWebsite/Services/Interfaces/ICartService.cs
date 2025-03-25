using GroceryWebsite.Models;

namespace GroceryWebsite.Services.Interfaces
{
    public interface ICartService
    {
        Cart GetCart();
        CartDetail AddToCart(int productId, int quantity);
        CartDetail UpdateCart(Dictionary<int, int> quantities);
        bool RemoveFromCart(int productId);
    }
}
