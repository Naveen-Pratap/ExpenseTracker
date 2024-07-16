using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseTracker.Controllers
{
    [Route("api/expense")]
    [ApiController]
    public class ExpenseController : Controller
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpenseById(int id)
        {
            //TODO: Handle does not exist
            return (await _db.Expenses.FindAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddExpense(Expense expense)
        {
            expense.CreatedTime = DateTime.Now;

            _db.Expenses.Attach(expense);
            await _db.SaveChangesAsync();

            return expense.Id;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> UpdateExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            _db.Entry(expense).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!await ExpenseExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return id;
        } 
        
        private async Task<bool> ExpenseExists(int id)
        {
            return await _db.Expenses.AnyAsync(e => e.Id == id);
        }

    }


}
