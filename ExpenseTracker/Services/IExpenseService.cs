
namespace ExpenseTracker.Services
{
    public interface IExpenseService
    {
        List<Expense> Expenses { get; set; }
        bool LoadingExpenses { get; set; }

        event Action OnChange;
        event Action? RefreshRequested;

        Task AddExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);
        Task EditExpenseAsync(int id, Expense expense);
        Task<Expense> GetExpenseByIdAsync(int id);
        Task LoadExpensesAsync();
    }
}