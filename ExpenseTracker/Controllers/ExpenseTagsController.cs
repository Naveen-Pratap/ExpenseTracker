using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;

namespace ExpenseTracker.Controllers
{
    [Route("api/expensetags")]
    [ApiController]
    public class ExpenseTagsController : Controller
    {
        private readonly ExpenseContext _db;

        public ExpenseTagsController(ExpenseContext db) => _db = db;

        public async Task<ActionResult<List<ExpenseTag>>> GetExpenseTags()
        {
            var expenseTags = await(_db.ExpenseTags.ToListAsync());
            return Ok(expenseTags);
        }
    }
}
