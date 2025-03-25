using GroceryWebsite.DTOs;
using GroceryWebsite.Models;

namespace GroceryWebsite.Services.Interfaces
{
    public interface IUserService
    {
        int GetUserId();
        User UpdateUser(int userId, UpdateUserRequest updateUserRequest);
    }
}
