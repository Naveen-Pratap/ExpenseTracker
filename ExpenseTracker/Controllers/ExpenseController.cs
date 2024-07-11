using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [Route("expense")]
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
    }
}
