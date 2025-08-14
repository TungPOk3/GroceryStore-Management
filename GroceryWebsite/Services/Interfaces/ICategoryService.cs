using GroceryWebsite.DTOs;
using GroceryWebsite.Models;

namespace GroceryWebsite.Services.Interfaces
{
    public interface ICategoryService
    {
        Category AddCategory(CreateCategoryRequest createCategoryRequest);
        Category UpdateCategory(int id, UpdateCategoryRequest updateCategoryRequest);
        bool DeleteCategory(int id);
    }
}
