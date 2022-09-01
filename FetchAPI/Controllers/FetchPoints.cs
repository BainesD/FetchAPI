using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FetchAPI.Models;
using FetchAPI.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FetchAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FetchPoints : ControllerBase
    {
        //Instance of the ApiContext allowing the Database to begin storing Transactions
        private readonly ApiContext _context;


        //custom constructor making sure to pass in the context 
        public FetchPoints(ApiContext context)
        {
            _context = context;
        }

        //Create and Edit Transactions
        [HttpPost]
        public JsonResult CreateEditTransaction(Transaction transaction)
        {
            //if transaction id is not set an id is assigned automaticaaly
            if (transaction.Id == 0 || transaction.Id == null)
            {
                //add transaction to the database
                _context.Transactions.Add(transaction);
            }
            else
            {
                //search to ensure transaction does not already exist
                var transactionInDb = _context.Transactions.Find(transaction.Id);
                //if transaction id is entered but no transaction exists returns not found
                if (transactionInDb == null)
                    return new JsonResult(NotFound());

                //update transaction to new information if id is present
                transactionInDb = transaction;
                _context.Transactions.Update(transactionInDb);
            }

            //save changes to Database
            _context.SaveChanges();

            //return a JSON confirming operation and showing result
            return new JsonResult(Ok(transaction));
        }

        //Create or Edit Multiple Transactions
        [HttpPost]
        public JsonResult CreateEditTransactionsBatch(IEnumerable<Transaction> transactions)
        {

            //List of transactions is passed in by user in the form of JSON
            foreach (var transaction in transactions)
            {
                //If no transaction id is specified transaction is added to Database
                if (transaction.Id == 0 || transaction.Id == null)
                {
                    var transactionModel = _context.Add(transaction);
                }
                else
                {
                    //search to ensure transaction does not already exist
                    var transactionInDb = _context.Transactions.Find(transaction.Id);
                    //if transaction id is entered but no transaction exists returns not found
                    if (transactionInDb == null)
                        return new JsonResult(NotFound());

                    //update transaction to new information if id is present
                    transactionInDb = transaction;
                    _context.Transactions.Update(transactionInDb);
                }
                //save changes to Database
                _context.SaveChanges();
            }
            //return a JSON confirming operation and showing result
            return new JsonResult(Ok(transactions));
        }


        //GET Transaction by id
        [HttpGet]
        public JsonResult Get(int id)
        {
            //sets the result variable to the transaction specified by the id
            var result = _context.Transactions.Find(id);

            //if no transaction exists return not found
            if (result == null)
                return new JsonResult(NotFound());

            //if transaction exists return operation successful with specified transaction
            return new JsonResult(Ok(result));
        }

        //DELETE Transaction by id
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            //result is the transaction specified by the id
            var result = _context.Transactions.Find(id);

            //if not existing returns not found
            if (result == null)
                return new JsonResult(NotFound());

            //if existing removes from database and saves changes made to database
            _context.Transactions.Remove(result);
            _context.SaveChanges();

            return new JsonResult(NoContent());
        }

        //GET All transactions
        [HttpGet]
        public JsonResult GetAll()
        {
            //adds all transactions to list
            var result = _context.Transactions.ToList();
            //if no transactions return no content
            if (result.Count == 0 || result == null)
                return new JsonResult(NoContent());

            return new JsonResult(Ok(result));
        }

        //Spend Points Route
        [HttpPost]
        public JsonResult SpendPoints(int points)
        {
            //add all transactions to a list
            var allTransactions = _context.Transactions.ToList();

            //create an empty list for storing completed transactions
            List<Transaction> transactionsCompleted = new List<Transaction>();

            //sorts all payers by name and gets sum of all transactions that have taken place for that payer
            List<Balance> pointsByPayer = allTransactions.GroupBy(t => t.Payer, (key, t) =>
            {
                var transactionArray = t as Transaction[] ?? t.ToArray();
                return new Balance()
                {
                    Payer = key,
                    Points = transactionArray.Sum(ta => ta.Points)

                };
            }).ToList();

            //while loop to ensure all points are spent
            while (points > 0)
            {
                //order all transactions by date, youngest to oldest
                var transactsYoungToOld = allTransactions.OrderBy(t => t.Timestamp);
                //go through each transaction and compare it to the points balance for the payer
                foreach (var transaction in transactsYoungToOld)
                {
                 

                    foreach (var payer in pointsByPayer)
                    {
                        //if the payers are the same, transaction points are less than the total payer points, and the payer points are not exhausted continue
                        if (transaction.Payer == payer.Payer && transaction.Points <= payer.Points && payer.Points != 0)
                        {
                            //foreach(var uniqueTransaction in allTransactions)
                            //{
                            //    if(uniqueTransaction.Points<0 && uniqueTransaction.Payer==transaction.Payer)
                            //    {
                            //        transaction.Points += uniqueTransaction.Points;
                            //    }
                            //}

                            //if the transaction points are positive or negative continue
                            if (transaction.Points > 0 || transaction.Points < 0)
                            {
                                // if the points left is less than the payer balance use what is available from the payer balance and continue
                                if (points < payer.Points)
                                {
                                    //create and add a new transaction to the in memory Database
                                    transactionsCompleted.Add(new Transaction() { Payer = transaction.Payer, Points = -points, Timestamp = DateTime.Now });
                                    _context.Transactions.Add(new Transaction() { Payer = transaction.Payer, Points = -points, Timestamp = DateTime.Now });
                                    //subtract points from the payer balance and the remaining points from the request, thus ending the while loop
                                    payer.Points -= points;
                                    points -= points;
                                    //save changes to database
                                    _context.SaveChanges();
                                }
                                //if the points left are greater than the payer balance continue
                                if (points >= payer.Points)
                                {
                                    //subtract the points of the transaction being referenced from the payer's total balance
                                    payer.Points -= transaction.Points;
                                    //subtract the points of the transaction from the points requested to spend
                                    points -= transaction.Points;
                                    //create and add the new transaction to the database
                                    transactionsCompleted.Add(new Transaction() { Payer = transaction.Payer, Points = -transaction.Points, Timestamp = DateTime.Now });
                                    _context.Transactions.Add(new Transaction() { Payer = transaction.Payer, Points = -transaction.Points, Timestamp = DateTime.Now });
                                    //save changes to database
                                    _context.SaveChanges();
                                }
                                
                            }
                        }
                    }
                }
            }
            //return that the operation was successful and give the list of new transactions made
            return new JsonResult(Ok(transactionsCompleted));

        }

        //GET Balances
        [HttpGet]
        public JsonResult GetBalances()
        {
            //get all transactions stored in the in memory database
            var allTransactions = _context.Transactions.ToList();

            //organize the payers by name and sum up the total points balance based on all transactions
            List<Balance> pointsByPayer = allTransactions.GroupBy(t => t.Payer, (key, t) =>
            {
                var transactionArray = t as Transaction[] ?? t.ToArray();
                return new Balance()
                {
                    Payer = key,
                    Points = transactionArray.Sum(ta => ta.Points)

                };
            }).ToList();

            //return operation successfull and give the balances of each payer
            return new JsonResult(Ok(pointsByPayer));
        }

    }
}
