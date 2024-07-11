using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseTag> Tags { get; set; }
    }
}
