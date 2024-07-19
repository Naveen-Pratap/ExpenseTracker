namespace ExpenseTracker.Utils
{
    public class Clock : IClock
    {
        public DateTime GetLocalTimeNow()
        {
            return DateTime.Now;
        }
    }
}
