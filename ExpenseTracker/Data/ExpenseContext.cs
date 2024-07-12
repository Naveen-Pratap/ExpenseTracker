using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseTag> ExpenseTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>().Property(e => e.CreatedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
