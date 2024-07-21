using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.WebRequestMethods;

namespace ExpenseTracker.Services
{
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

        public async Task LoadExpenseTagsAsync()
        {
            LoadingExpenseTags = true;
            ExpenseTags = await _httpClient.GetFromJsonAsync<List<ExpenseTag>>("api/expensetags");
            LoadingExpenseTags = false;
        }
    }
}
