namespace ExpenseTracker
{
    /// <summary>
    /// Represents an expense
    /// </summary>
    public class Expense
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
        public ExpenseTag Tag { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}
