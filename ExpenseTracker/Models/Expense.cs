using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker
{
    /// <summary>
    /// Represents an expense
    /// </summary>
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid cost > 0")]
        public int Value { get; set; }
        [Required]
        public string Description { get; set; }
        public ExpenseTag Tag { get; set; }

        [Required]
        public int Tagid { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}
