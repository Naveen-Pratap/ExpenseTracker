﻿
namespace ExpenseTracker.Services
{
    public interface IExpenseAnalyticsService
    {
        Task<Dictionary<string, int>> GetTotalExpenseByTagAsync(DateTime startDate);
        Task<int> GetTotalExpenseAsync(DateTime startDate);
    }
}