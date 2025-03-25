using GroceryWebsite.Data;
using GroceryWebsite.Models;
using GroceryWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GroceryWebsite.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public OrderService(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        public Order GetOrder(int typeShipping)
        {
            var userId = _userService.GetUserId();
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.CartDetails.Any()) return null;

            string typeShipingName;
            if (typeShipping == 1)
            {
                typeShipingName = "Khách lấy tận tay";
            }
            else if (typeShipping == 2)
            {
                typeShipingName = "Giao hàng tận nơi";
            }
            else
            {
                throw new ArgumentException("Phương thức giao hàng không hợp lệ.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.CartDetails.Sum(c => c.Product.Price * c.Quantity),
                ShippingMethod = typeShipingName,
                OrderDetails = cart.CartDetails.Select(cd => new OrderDetail
                {
                    ProductId = cd.ProductId,
                    Price = cd.Price,
                    Quantity = cd.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            _context.CartDetails.RemoveRange(cart.CartDetails);
            _context.SaveChanges();

            return order;
        }

        public List<Order> GetListOrders()
        {
            var userId = _userService.GetUserId();
            return _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .ToList();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.OrderStatus = status;
                _context.SaveChanges();
            }
        }
    }
}
