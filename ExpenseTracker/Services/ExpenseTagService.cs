namespace ExpenseTracker.Services
{
    /// <summary>
    /// Service for handling tags related business logic.
    /// </summary>
    public class ExpenseTagService : IExpenseTagService
    {
        private readonly HttpClient? _httpClient;
        public bool LoadingExpenseTags { get; set; }

        public ExpenseTagService(HttpClient? httpClient)
        {
            _httpClient = httpClient;
            LoadingExpenseTags = false;
        }

        public List<ExpenseTag> ExpenseTags { get; set; }

        /// <summary>
        /// Loads Tags into ExpenseTags. LoadingExpenseTags can be used by ui components for
        /// displaying loader etc.
        /// </summary>
        /// <returns></returns>
        public async Task LoadExpenseTagsAsync()
        {
            LoadingExpenseTags = true;
            ExpenseTags = await _httpClient.GetFromJsonAsync<List<ExpenseTag>>("api/expensetags");
            LoadingExpenseTags = false;
        }
    }
}
