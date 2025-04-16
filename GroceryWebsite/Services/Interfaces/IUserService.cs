using GroceryWebsite.DTOs;
using GroceryWebsite.Models;

namespace GroceryWebsite.Services.Interfaces
{
    public interface IUserService
    {
        int GetUserId();
        User UpdateUser(UpdateUserRequest updateUserRequest);
        Task ResetPasswordAsync(string email);
        User ChangePassword(UpdatePasswordRequest updatePasswordRequest);
    }
}
