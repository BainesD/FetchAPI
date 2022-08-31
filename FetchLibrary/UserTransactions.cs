using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchLibrary
{
    public static class UserTransactions
    {
        public static List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public static void CreateTransaction()
        {
            var transaction = new Transaction();
            var transactionId = Transactions.OrderByDescending(t => t.TransactId).First().TransactId;
            transaction.TransactId = transactionId++;
        }
    }
}
