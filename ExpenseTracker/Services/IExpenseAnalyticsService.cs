
namespace ExpenseTracker.Services
{
    /// <summary>
    /// Interface for ExpenseAnalytics service.
    /// </summary>
    public interface IExpenseAnalyticsService
    {
        Task<Dictionary<string, int>> GetTotalExpenseByTagAsync(DateTime startDate);
        Task<int> GetTotalExpenseAsync(DateTime startDate);
    }
}