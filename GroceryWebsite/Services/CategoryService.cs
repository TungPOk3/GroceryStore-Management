using GroceryWebsite.Data;
using GroceryWebsite.DTOs;
using GroceryWebsite.Models;

namespace GroceryWebsite.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public Category AddCategory(CreateCategoryRequest createCategoryRequest)
        {
            var category = new Category
            {
                CategoryName = createCategoryRequest.CategoryName,
                Description = createCategoryRequest.Description
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category;
        }

        public Category UpdateCategory(int id, UpdateCategoryRequest updateCategoryRequest)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            category.CategoryName = updateCategoryRequest.CategoryName;
            category.Description = updateCategoryRequest.Description;

            _context.SaveChanges();
            return category;
        }

        public bool DeleteCategory(int id) 
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }
    }
}
