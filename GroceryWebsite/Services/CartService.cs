using GroceryWebsite.Data;
using GroceryWebsite.Models;
using GroceryWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GroceryWebsite.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public CartService(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public Cart GetCart()
        {
            var userId = _userService.GetUserId();
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Product)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            return cart;
        }

        public CartDetail AddToCart(int productId, int quantity)
        {
            var cart = GetCart();

            var cartDetail = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);

            if (cartDetail == null)
            {
                cartDetail = new CartDetail
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Price = _context.Products.Find(productId).Price,
                    Quantity = quantity
                };

                _context.CartDetails.Add(cartDetail);
            }
            else
            {
                cartDetail.Quantity += quantity;
            }

            _context.SaveChanges();
            return cartDetail;
        }

        public CartDetail UpdateCart(Dictionary<int, int> quantities)
        {
            var cart = GetCart();
            var cartDetail = new CartDetail();

            foreach (var (productId, quantity) in quantities)
            {
                cartDetail = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);

                if (cartDetail != null)
                {
                    cartDetail.Quantity = quantity;
                }else
                {
                    throw new Exception("Sản phẩm không tồn tại trong giỏ hàng.");
                }    
            }

            _context.SaveChanges();
            return cartDetail;
        }
        public bool RemoveFromCart(int productId)
        {
            var cart = GetCart();

            var cartDetail = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);

            if (cartDetail != null)
            {
                _context.CartDetails.Remove(cartDetail);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
