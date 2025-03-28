namespace CargoPayWebApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}