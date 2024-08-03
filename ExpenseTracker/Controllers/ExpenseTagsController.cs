using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [Route("api/expensetags")]
    [ApiController]
    /// <summary>
    /// Controller for Expense Tags related apis.
    /// </summary>
    public class ExpenseTagsController : Controller
    {
        private readonly ExpenseContext _db;

        public ExpenseTagsController(ExpenseContext db) => _db = db;

        public async Task<ActionResult<List<ExpenseTag>>> GetExpenseTags()
        {
            var expenseTags = await (_db.ExpenseTags.ToListAsync());
            return Ok(expenseTags);
        }
    }
}
