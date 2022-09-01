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
        private readonly ApiContext _context;

        public FetchPoints(ApiContext context)
        {
            _context = context;
        }

        //Create and Edit Transactions
        [HttpPost]
        public JsonResult CreateEditTransaction(Transaction transaction)
        {
            if (transaction.Id == 0 || transaction.Id == null)
            {
                _context.Transactions.Add(transaction);
            }
            else
            {
                var transactionInDb = _context.Transactions.Find(transaction.Id);
                if (transactionInDb == null)
                    return new JsonResult(NotFound());

                transactionInDb = transaction;
            }

            _context.SaveChanges();

            return new JsonResult(Ok(transaction));
        }

        //Create or Edit Multiple
        [HttpPost]
        public JsonResult CreateEditTransactionsBatch(IEnumerable<Transaction> transactions)
        {

            foreach( var transaction in transactions)
            {
               
                    if (transaction.Id == 0 || transaction.Id == null) {

                        var transactionModel = _context.Add(transaction);
                        

                    }
                    else
                    {
                        var transactionInDb = _context.Transactions.Find(transaction.Id);
                        if (transactionInDb == null)
                            return new JsonResult(NotFound());

                        transactionInDb = transaction;
                    }
                    _context.SaveChanges();
                
            }

            return new JsonResult(Ok(transactions));
        }

        //GET Transactions
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Transactions.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result)); 
        }

        //DELETE Transactions
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Transactions.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            _context.Transactions.Remove(result);
            _context.SaveChanges();

            return new JsonResult(NoContent());
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.Transactions.ToList();
            return new JsonResult(Ok(result));
        }

    }
}
