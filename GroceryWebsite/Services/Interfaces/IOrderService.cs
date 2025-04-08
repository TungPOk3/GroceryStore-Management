using GroceryWebsite.Models;

namespace GroceryWebsite.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetOrderbyId(int id);
        Order GetOrder(int shippingMethod);
        List<Order> GetListOrders();
        void UpdateOrderStatus(int orderId, string status);
    }
}
