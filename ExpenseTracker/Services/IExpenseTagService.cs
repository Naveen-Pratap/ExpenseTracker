
namespace ExpenseTracker.Services
{
    /// <summary>
    /// Interface for expense tag service.
    /// </summary>
    public interface IExpenseTagService
    {
        List<ExpenseTag> ExpenseTags { get; set; }
        bool LoadingExpenseTags { get; set; }

        Task LoadExpenseTagsAsync();
    }
}