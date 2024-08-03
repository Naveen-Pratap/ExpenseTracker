namespace ExpenseTracker.Utils
{
    /// <summary>
    /// Clock class to do time based operations. Implements IClock to
    /// make it easier to mock.
    /// </summary>
    public class Clock : IClock
    {
        public DateTime GetLocalTimeNow()
        {
            return DateTime.Now;
        }
    }
}
