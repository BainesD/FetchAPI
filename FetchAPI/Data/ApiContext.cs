using Microsoft.EntityFrameworkCore;
using FetchAPI.Models;

namespace FetchAPI.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options): base(options)
        {

        }
    }
}
