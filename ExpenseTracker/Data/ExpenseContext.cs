using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    /// <summary>
    /// EF core dbcontext for Expenses.
    /// </summary>
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions options) : base(options)
        {
        }

        public ExpenseContext() { }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseTag> ExpenseTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>().Property(e => e.CreatedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
