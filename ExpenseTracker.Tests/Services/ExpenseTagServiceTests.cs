﻿using ExpenseTracker.Services;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Services
{
    public class ExpenseTagServiceTests
    {
        [Fact]
        public async Task LoadExpenseTagsAsync_LoadTagsCorrectly()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/api/expensetags")
                    .Respond("application/json", "[{\"id\":1,\"Name\":\"dummy\"}]");

            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri("http://localhost");
            var expenseTagService = new ExpenseTagService(client);

            // Act
            await expenseTagService.LoadExpenseTagsAsync();

            // Assert
            Assert.Single(expenseTagService.ExpenseTags);
            Assert.Equal(1, expenseTagService.ExpenseTags[0].Id);
        }
    }
}