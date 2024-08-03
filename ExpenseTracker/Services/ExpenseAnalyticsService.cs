using ExpenseTracker.Repositories;

namespace ExpenseTracker.Services
{
    public class ExpenseAnalyticsService : IExpenseAnalyticsService
    {
        private IExpenseRepository _expenseRepository;
        public ExpenseAnalyticsService(IExpenseRepository expenseRepository) 
        {
            _expenseRepository = expenseRepository;
        }
        public async Task<Dictionary<string, int>> GetTotalExpenseByTagAsync(DateTime startDate)
        {
            var expensesAfterStartDate = (await _expenseRepository.GetAllAsync()).ToList().Where(e => e.CreatedTime >= startDate );
            var TagTotalExpenseMap = expensesAfterStartDate
            .GroupBy(e => e.Tag.Name)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Value));

            return TagTotalExpenseMap;
        }

        public async Task<int> GetTotalExpenseAsync(DateTime startDate)
        {

            var expensesAfterStartDate = (await _expenseRepository.GetAllAsync()).ToList().Where(e => e.CreatedTime >= startDate);
            var totalExpense = expensesAfterStartDate.Sum(e => e.Value);

            return totalExpense;
        }
    }
}
