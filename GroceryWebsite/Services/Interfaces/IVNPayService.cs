namespace GroceryWebsite.Services.Interfaces
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(int orderId, decimal amount);
    }
}
