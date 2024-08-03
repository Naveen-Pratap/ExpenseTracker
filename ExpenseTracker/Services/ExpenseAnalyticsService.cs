using ExpenseTracker.Repositories;

namespace ExpenseTracker.Services
{
    /// <summary>
    /// Service which holds analytics based business logic.
    /// </summary>
    public class ExpenseAnalyticsService : IExpenseAnalyticsService
    {
        private IExpenseRepository _expenseRepository;
        public ExpenseAnalyticsService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        /// <summary>
        /// Get distribution of total expenses per tag for expenses created after startDate.
        /// </summary>
        /// <param name="startDate"> Start date from which total expenses are calculated </param>
        /// <returns> A dictionary of tag -> Total expense for that tag </returns>
        public async Task<Dictionary<string, int>> GetTotalExpenseByTagAsync(DateTime startDate)
        {
            var expensesAfterStartDate = (await _expenseRepository.GetAllAsync()).ToList().Where(e => e.CreatedTime >= startDate);
            var TagTotalExpenseMap = expensesAfterStartDate
            .GroupBy(e => e.Tag.Name)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Value));

            return TagTotalExpenseMap;
        }
        /// <summary>
        /// Get total expenses from start date.
        /// </summary>
        /// <param name="startDate"> Start date from which total expenses are calculated </param>
        /// <returns>Total expense </returns>
        public async Task<int> GetTotalExpenseAsync(DateTime startDate)
        {

            var expensesAfterStartDate = (await _expenseRepository.GetAllAsync()).ToList().Where(e => e.CreatedTime >= startDate);
            var totalExpense = expensesAfterStartDate.Sum(e => e.Value);

            return totalExpense;
        }
    }
}
