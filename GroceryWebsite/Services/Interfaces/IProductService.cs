using GroceryWebsite.DTOs;
using GroceryWebsite.Models;

namespace GroceryWebsite.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        List<Product> SearchProduct(string str);
        Product AddProduct(CreateProductRequest createProductRequest);
        Product UpdateProduct(int id, UpdateProductRequest updateProductRequest);
        bool DeleteProduct(int id);
    }
}
