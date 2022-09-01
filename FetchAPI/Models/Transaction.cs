namespace FetchAPI.Models
{
    public class Transaction
    {
        //Each transaction should have a payer, timestamp, points, and unique id
        public int Id { get; set; }
        public string Payer { get; set; }
        public DateTime Timestamp { get; set; }
        public int Points { get; set; }

        //The default constructor is used in every call so far, but having a custom constructor allows for scalability into a user friendly app
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