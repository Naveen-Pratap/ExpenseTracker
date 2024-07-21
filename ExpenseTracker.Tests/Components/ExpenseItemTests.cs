using System;
using System.Net.Http;
using ExpenseTracker.Components;
using ExpenseTracker.Services;
using Moq;
using RichardSzalay.MockHttp;

namespace ExpenseTracker.Tests.Components
{

    public class ExpenseItemTests : TestContext
    {
        
        [Fact]
        public void ExpenseItem_DisplaysCorrectInfo()
        {
            // Arrange
            var expense = new Expense
            {
                Id = 1,
                Value = 100,
                Description = "Test",
                CreatedTime = new DateTime(2024, 09, 3, 9, 6, 13),
            };

            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            // Act
            var cut = RenderComponent<ExpenseItem>(parameters => parameters.Add(p => p.expense, expense));
            
            var valueElem = cut.Find(".value");
            var descriptionElem = cut.Find(".description");
            var createdTimeElem = cut.Find(".created-time");
            var editIconElem = cut.Find(".edit-icon");
            var deleteIconElem = cut.Find(".delete-icon");

            // Assert
            Assert.Equal(expense.Value.ToString(), valueElem.TextContent.Trim());
            Assert.Equal(expense.Description.ToString(), descriptionElem.TextContent.Trim());
            Assert.Equal(expense.CreatedTime.ToString(), createdTimeElem.TextContent.Trim());
            Assert.NotNull(editIconElem);
            Assert.NotNull(deleteIconElem);
        }

    }
}
