using GroceryWebsite.Data;
using GroceryWebsite.DTOs;
using GroceryWebsite.Models;
using GroceryWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GroceryWebsite.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public List<Product> SearchProduct(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return _context.Products.ToList();
            }

            return _context.Products
                .Where(p => EF.Functions.Like(p.ProductName, $"%{str}%")).ToList();
        }

        public Product AddProduct(CreateProductRequest createProductRequest)
        {
            var product = new Product
            {
                ProductName = createProductRequest.ProductName,
                Description = createProductRequest.Description,
                Price = createProductRequest.Price,
                Stock = createProductRequest.Stock,
                Image = createProductRequest.Image,
                CategoryId = createProductRequest.CategoryId
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        public Product UpdateProduct(int id, UpdateProductRequest updateProductRequest) 
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            product.ProductName = updateProductRequest.ProductName;
            product.Description = updateProductRequest.Description;
            product.Price = updateProductRequest.Price;
            product.Stock = updateProductRequest.Stock;
            product.Image = updateProductRequest.Image;
            product.CategoryId = updateProductRequest.CategoryId;

            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public bool DeleteProduct(int id) 
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }
    }
}
