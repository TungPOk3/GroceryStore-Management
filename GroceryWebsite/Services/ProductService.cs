using GroceryWebsite.Data;
using GroceryWebsite.DTOs;
using GroceryWebsite.Models;

namespace GroceryWebsite.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
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
