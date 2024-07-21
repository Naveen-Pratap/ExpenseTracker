
namespace ExpenseTracker.Services
{
    public interface IExpenseTagService
    {
        List<ExpenseTag> ExpenseTags { get; set; }
        bool LoadingExpenseTags { get; set; }

        Task LoadExpenseTagsAsync();
    }
}