using Microsoft.EntityFrameworkCore;
using CargoPayWebApi.Models;
using CargoPayWebApi.Data;

namespace CargoPayWebApi.Services
{
    public class CardService : ICardService
    {
        private readonly CargoPayDbContext _context;
        private readonly UFEService _ufeService;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CardService(CargoPayDbContext context)
        {
            _context = context;
            _ufeService = UFEService.Instance;
        }

        public async Task<Card> CreateCardAsync(string cardNumber)
        {
            if (cardNumber.Length != 15 || !cardNumber.All(char.IsDigit))
                throw new ArgumentException("Card number must be 15 digits");

            var card = new Card
            {
                CardNumber = cardNumber,
                Balance = 0,
                CreatedAt = DateTime.UtcNow,
                Transactions = new List<Transaction>()
            };

            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<bool> ProcessPaymentAsync(string cardNumber, decimal amount)
        {
            await _semaphore.WaitAsync();
            try
            {
                var card = await _context.Cards
                    .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

                if (card == null)
                    throw new ArgumentException("Card not found");

                var fee = _ufeService.GetCurrentFee();
                var feeAmount = amount * fee;
                var totalAmount = amount + feeAmount;

                if (card.Balance < totalAmount)
                    throw new InvalidOperationException($"Insufficient funds. Required amount: {totalAmount:C2} (Payment: {amount:C2}, Fee: {feeAmount:C2}, Current fee rate: {fee:P2})");

                card.Balance -= totalAmount;

                var transaction = new Transaction
                {
                    CardId = card.Id,
                    Amount = amount,
                    Fee = fee,
                    TransactionDate = DateTime.UtcNow
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<decimal> GetBalanceAsync(string cardNumber)
        {
            var card = await _context.Cards
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

            if (card == null)
                throw new ArgumentException("Card not found");

            return card.Balance;
        }

        public async Task<decimal> LoadCardAsync(string cardNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            await _semaphore.WaitAsync();
            try
            {
                var card = await _context.Cards
                    .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

                if (card == null)
                    throw new ArgumentException("Card not found");

                card.Balance += amount;

                var transaction = new Transaction
                {
                    CardId = card.Id,
                    Amount = amount,
                    Fee = 0, 
                    TransactionDate = DateTime.UtcNow
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                
                return card.Balance;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}