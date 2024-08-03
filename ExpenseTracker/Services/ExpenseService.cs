namespace ExpenseTracker.Services
{
    /// <summary>
    /// Service which Expense related business logic. I wanted to mess around with backend-for-frontend design
    /// which is why we're using apis instead of directly calling EF core.
    /// </summary>
    public class ExpenseService : IExpenseService
    {
        public event Action? RefreshRequested;
        private readonly HttpClient _httpClient;
        public bool LoadingExpenses { get; set; }
        public event Action OnChange;
        public List<Expense> Expenses { get; set; }

        public ExpenseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            LoadingExpenses = false;

        }

        /// <summary>
        /// Invokes delegates supplied by various blazor components on a state change.
        /// </summary>
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        /// <summary>
        /// Loads expenses to Expenses attribute. LoadingExpenses can be used by ui components
        /// to display loader etc. Updates Expenses and triggers all the state change related delgates.
        /// </summary>
        /// <returns></returns>
        public async Task LoadExpensesAsync()
        {

            LoadingExpenses = true;
            Expenses = await _httpClient.GetFromJsonAsync<List<Expense>>("api/expense");
            LoadingExpenses = false;
            NotifyStateChanged();
        }
        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            var expense = await _httpClient.GetFromJsonAsync<Expense>($"api/expense/{id}");
            return expense;

        }

        public async Task DeleteExpenseAsync(int id)
        {
            var resp = await _httpClient.DeleteAsync($"api/expense/{id}");
            if (resp.IsSuccessStatusCode)
            {
                await LoadExpensesAsync();
            }

        }

        public async Task EditExpenseAsync(int id, Expense expense)
        {
            var resp = await _httpClient.PutAsJsonAsync($"api/expense/{id}", expense);
            if (resp.IsSuccessStatusCode)
            {
                await LoadExpensesAsync();
            }
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            var resp = await _httpClient.PostAsJsonAsync("api/expense", expense);
            if (resp.IsSuccessStatusCode)
            {
                await LoadExpensesAsync();
            }
        }

    }
}
