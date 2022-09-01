using Microsoft.EntityFrameworkCore;
using FetchAPI.Models;

namespace FetchAPI.Data
{
    public class ApiContext : DbContext
    {
        //The ApiContext allows for in memory storage of JSON information

        //Transactions serves as the in memory database storing any information passed through the api
        public DbSet<Transaction> Transactions { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options): base(options)
        {

        }
    }
}
