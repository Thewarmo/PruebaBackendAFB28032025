namespace CargoPayWebApi.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public class CardNumberRequest
    {
        public required string CardNumber { get; set; }
    }
}