using ExpenseTracker.Repositories;
using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseTracker.Controllers
{
    [Route("api/expense")]
    [ApiController]

    /// <summary>
    /// Controller for expense apis
    /// </summary>
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IClock _clock;

        public ExpenseController(IExpenseRepository expenseRepository, IClock clock)
        {
            _expenseRepository = expenseRepository;
            _clock = clock;
        }


        [HttpGet]
        public async Task<ActionResult<List<Expense>>> GetExpenses()
        {
            return (await _expenseRepository.GetAllAsync()).OrderByDescending(s => s.Id).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpenseById(int id)
        {
            //TODO: Handle does not exist
            var expense = await _expenseRepository.GetExpenseByIdAsync(id);
            return expense;
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> AddExpense(Expense expense)
        {
            expense.CreatedTime = _clock.GetLocalTimeNow();

            await _expenseRepository.AddExpenseAsync(expense);

            return expense;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Expense>> UpdateExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            try
            {
                await _expenseRepository.UpdateExpenseAsync(expense);
            }

            catch (DbUpdateConcurrencyException)
            {
                if (await _expenseRepository.GetExpenseByIdAsync(id) == null)
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return expense;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Expense>> DeleteExpense(int id)
        {
            Expense expense = await _expenseRepository.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                return BadRequest();
            }

            await _expenseRepository.DeleteExpenseAsync(expense);

            return expense;
        }

    }


}
