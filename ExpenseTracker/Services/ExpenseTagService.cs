using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.WebRequestMethods;

namespace ExpenseTracker.Services
{
    public class ExpenseTagService
    {
        private readonly HttpClient? _httpClient;
        private readonly NavigationManager? _navigationManager;
        public bool LoadingExpenseTags { get; set; }

        public ExpenseTagService(HttpClient? httpClient, NavigationManager? navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            LoadingExpenseTags = false;
        }

        public List<ExpenseTag> ExpenseTags { get; set; }

        public async Task LoadExpenseTagsAsync()
        {
            LoadingExpenseTags = true;
            ExpenseTags = await _httpClient.GetFromJsonAsync<List<ExpenseTag>>(_navigationManager.BaseUri + "api/expensetags");
            LoadingExpenseTags = false;
        }
    }
}
