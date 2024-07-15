using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [Route("api/expense")]
    [ApiController]
    public class ExpenseController: Controller
    {
        private readonly ExpenseContext _db;

        public ExpenseController(ExpenseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Expense>>> GetExpenses()
        {
            return (await _db.Expenses.ToListAsync()).OrderByDescending(s => s.Id).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddExpense(Expense expense)
        {
            expense.CreatedTime = DateTime.Now;

            _db.Expenses.Attach(expense);
            await _db.SaveChangesAsync();

            return expense.Id;
        }
    }
}
