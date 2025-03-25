using GroceryWebsite.Models;

namespace GroceryWebsite.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetOrder(int shippingMethod);
        List<Order> GetListOrders();
        void UpdateOrderStatus(int orderId, string status);
    }
}
