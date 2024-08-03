using ExpenseTracker.Services;
using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Services
{
    public class ExpenseServiceTests
    {
        [Fact]
        public async Task LoadExpensesAsync_LoadsExpensesAndNotifyStateChanged()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/api/expense")
                    .Respond("application/json", "[{\"id\":1,\"value\":1,\"description\":\"eggs\",\"tag\":null,\"tagid\":2,\"createdTime\":\"2024-07-20T01:20:24.8326019\"}]");

            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri("http://localhost");
            var expenseService = new ExpenseService(client);
            bool stateChangedInvoked = false;
            expenseService.OnChange += () => stateChangedInvoked = true;

            // Act
            await expenseService.LoadExpensesAsync();

            // Assert
            Assert.Single(expenseService.Expenses);
            Assert.Equal(1, expenseService.Expenses[0].Id);
            Assert.True(stateChangedInvoked);
        }

        [Fact]
        public async Task LoadExpensesAsync_ThrowsHttpException()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://localhost/api/expense").Throw(new HttpRequestException());
            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri("http://localhost");
            var expenseService = new ExpenseService(client);

            // Act
            Func<Task> action = async () => await expenseService.LoadExpensesAsync();

            // Assert
            await Assert.ThrowsAsync<HttpRequestException>(action);

        }

        [Fact]
        public async Task GetExpenseByIdAsync_ReturnsCorrectValue()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/api/expense/1")
                    .Respond("application/json", "{\"id\":1,\"value\":1,\"description\":\"eggs\",\"tag\":null,\"tagid\":2,\"createdTime\":\"2024-07-20T01:20:24.8326019\"}");

            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri("http://localhost");
            var expenseService = new ExpenseService(client);

            // Act
            var resp = await expenseService.GetExpenseByIdAsync(1);

            // Assert
            Assert.IsType<Expense>(resp);
            Assert.Equal(1, resp.Id);
        }

        //[Fact]
        //public async Task GetExpenseByIdAsync_HandleNullAPiResponse()
        //{
        //    // Arrange
        //    var mockHttp = new MockHttpMessageHandler();

        //    mockHttp.When("http://localhost/api/expense/1")
        //            .Respond(HttpStatusCode.NoContent);

        //    var client = new HttpClient(mockHttp);
        //    client.BaseAddress = new Uri("http://localhost");
        //    var expenseService = new ExpenseService(client);

        //    // Act
        //    var resp = await expenseService.GetExpenseByIdAsync(1);

        //    // Assert
        //    Assert.Null(resp);
        // }

        [Fact]
        public async Task DeleteExpenseAsync_LoadsExpenseOnSuccess()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/api/expense/1")
                    .Respond(HttpStatusCode.OK);
            mockHttp.When("http://localhost/api/expense")
                    .Respond("application/json", "[{\"id\":2,\"value\":1,\"description\":\"eggs\",\"tag\":null,\"tagid\":2,\"createdTime\":\"2024-07-20T01:20:24.8326019\"}]");

            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri("http://localhost");
            var expenseService = new ExpenseService(client);

            // Act
            await expenseService.DeleteExpenseAsync(1);

            // Assert
            Assert.Single(expenseService.Expenses);
            Assert.Equal(2, expenseService.Expenses[0].Id);

        }

        [Fact]
        public async Task EditExpenseAsync_LoadsExpenseOnSuccess()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/api/expense/1")
                    .Respond(HttpStatusCode.OK);
            mockHttp.When("http://localhost/api/expense")
                    .Respond("application/json", "[{\"id\":1,\"value\":1,\"description\":\"eggs-new\",\"tag\":null,\"tagid\":2,\"createdTime\":\"2024-07-20T01:20:24.8326019\"}]");

            var client = new HttpClient(mockHttp);
            Expense expense = new Expense { Id = 1, Value = 1, Description = "eggs-new" };
            client.BaseAddress = new Uri("http://localhost");
            var expenseService = new ExpenseService(client);

            // Act
            await expenseService.EditExpenseAsync(1, expense);

            // Assert
            Assert.Single(expenseService.Expenses);
            Assert.Equal(1, expenseService.Expenses[0].Id);
            Assert.Equal("eggs-new", expenseService.Expenses[0].Description);

        }

        [Fact]
        public async Task AddExpenseAsync_LoadsExpenseOnSuccess()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/api/expense")
                .WithContent("{'Id': 1}")
                .Respond(HttpStatusCode.OK);
            mockHttp.When("http://localhost/api/expense")
                    .Respond("application/json", "[{\"id\":1,\"value\":1,\"description\":\"eggs\",\"tag\":null,\"tagid\":2,\"createdTime\":\"2024-07-20T01:20:24.8326019\"}]");

            var client = new HttpClient(mockHttp);
            Expense expense = new Expense { Id = 1, Value = 1, Description = "eggs-new" };
            client.BaseAddress = new Uri("http://localhost");
            var expenseService = new ExpenseService(client);

            // Act
            await expenseService.AddExpenseAsync(expense);

            // Assert
            Assert.Single(expenseService.Expenses);
            Assert.Equal(1, expenseService.Expenses[0].Id);

        }
    }
}
