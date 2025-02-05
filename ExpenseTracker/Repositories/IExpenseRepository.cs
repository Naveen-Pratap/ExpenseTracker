﻿
namespace ExpenseTracker.Repositories
{
    /// <summary>
    /// Repository interface to be used for all database operations.
    /// </summary>
    public interface IExpenseRepository
    {
        Task<Expense> AddExpenseAsync(Expense expense);
        Task<Expense> DeleteExpenseAsync(Expense expense);
        Task<List<Expense>> GetAllAsync();
        Task<Expense> GetExpenseByIdAsync(int id);
        Task<Expense> UpdateExpenseAsync(Expense expense);
    }
}