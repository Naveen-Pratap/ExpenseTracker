
namespace ExpenseTracker.Utils
{
    /// <summary>
    /// Interface for handling time related operations
    /// so that they are easy to mock and unittest.
    /// </summary>
    public interface IClock
    {
        DateTime GetLocalTimeNow();
    }
}