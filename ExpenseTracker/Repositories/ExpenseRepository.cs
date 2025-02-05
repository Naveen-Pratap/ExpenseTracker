﻿using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    /// <summary>
    /// Repository class for Expense, use this as an abstraction for the database.
    /// </summary>
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseContext _context;

        public ExpenseRepository(ExpenseContext expenseContext)
        {
            _context = expenseContext;
        }

        public async Task<List<Expense>> GetAllAsync()
        {
            return await _context.Expenses.Include(entity => entity.Tag).ToListAsync();
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.Include(entity => entity.Tag).FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            _context.Expenses.Attach(expense);
            await _context.SaveChangesAsync();

            return expense;

        }

        public async Task<Expense> UpdateExpenseAsync(Expense expense)
        {
            _context.Entry(expense).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return expense;
        }

        public async Task<Expense> DeleteExpenseAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return expense;
        }
    }


}
