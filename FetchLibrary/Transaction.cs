﻿namespace FetchLibrary
{
    public class Transaction
    {
        public long Id { get; set; }
        public string PayerName { get; set; }
        public DateTime Timestamp { get; set; }
        public int Points { get; set; }

        public Transaction()
        {

        }

        public Transaction(long id, string name, DateTime timestamp, int points)
        {
            Id = id;
            PayerName = name;
            Timestamp = timestamp;
            Points = points;

        }
    }
}