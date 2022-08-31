namespace FetchLibrary
{
    public class Transaction
    {
        public int TransactId { get; set; }
        public string PayerName { get; set; }
        public DateTime Timestamp { get; set; }
        public int Points { get; set; }

        public Transaction()
        {

        }

        public Transaction(int id, string name, DateTime timestamp, int points)
        {
            TransactId = id;
            PayerName = name;
            Timestamp = timestamp;
            Points = points;

        }
    }
}