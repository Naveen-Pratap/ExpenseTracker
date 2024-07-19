using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.WebRequestMethods;

namespace ExpenseTracker.Services
{
    public class ExpenseService
    {
        public event Action? RefreshRequested;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        public bool LoadingExpenses = false;
        public event Action OnChange;
        public List<Expense> Expenses { get; set; }

        public ExpenseService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        

        public async Task LoadExpensesAsync()
        {
            LoadingExpenses = true;
            Expenses = await _httpClient.GetFromJsonAsync<List<Expense>>(_navigationManager.BaseUri + "api/expense");
            LoadingExpenses = false;
            NotifyStateChanged();
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            var expense = await _httpClient.GetFromJsonAsync<Expense>(_navigationManager.BaseUri + $"api/expense/{id}");
            return expense;

        }

        public async Task DeleteExpenseAsync(int id)
        {
            var resp = await _httpClient.DeleteAsync(_navigationManager.BaseUri + $"api/expense/{id}");
            if (resp.IsSuccessStatusCode)
            {
                await LoadExpensesAsync();
            }
            
        }

        public async Task EditExpenseAsync(int id, Expense expense)
        {
            var resp = await _httpClient.PutAsJsonAsync(_navigationManager.BaseUri + $"api/expense/{id}", expense);
            if (resp.IsSuccessStatusCode)
            {
                await LoadExpensesAsync();
            }
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            var resp = await _httpClient.PostAsJsonAsync(_navigationManager.BaseUri + "api/expense", expense);
            if (resp.IsSuccessStatusCode)
            {
                await LoadExpensesAsync();
            }
        }

    }
}
