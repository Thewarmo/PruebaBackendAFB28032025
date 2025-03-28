using CargoPayWebApi.Models;

namespace CargoPayWebApi.Services
{
    public interface ICardService
    {
        Task<Card> CreateCardAsync(string cardNumber);
        Task<bool> ProcessPaymentAsync(string cardNumber, decimal amount);
        Task<decimal> GetBalanceAsync(string cardNumber);
        Task<decimal> LoadCardAsync(string cardNumber, decimal amount);
    }
}