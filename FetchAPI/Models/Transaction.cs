namespace FetchAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Payer { get; set; }
        public DateTime Timestamp { get; set; }
        public int Points { get; set; }

        public Transaction()
        {

        }

        public Transaction(int id, string name, DateTime timestamp, int points)
        {
            Id = id;
            Payer = name;
            Timestamp = timestamp;
            Points = points;

        }
    }
}