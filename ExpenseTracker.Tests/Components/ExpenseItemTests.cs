using ExpenseTracker.Components;
using ExpenseTracker.Services;
using Moq;
using System;

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

        [Fact]
        public void ExpenseItem_EditButtonClickNavigatesCorrectly()
        {
            // Arrange
            Expense expense = new Expense { Id = 1 };
            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            // Act
            var cut = RenderComponent<ExpenseItem>(parameters => parameters.Add(p => p.expense, expense));
            cut.Find(".edit-icon").Click();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            // Assert
            Assert.Equal($"http://localhost/edit-expense/{1}", navMan.Uri);
        }

        [Fact]
        public void ExpenseItem_DeleteButtonClickCallsExpenseService()
        {
            // Arrange
            Expense expense = new Expense { Id = 1 };
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.DeleteExpenseAsync(1));
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            // Act
            var cut = RenderComponent<ExpenseItem>(parameters => parameters.Add(p => p.expense, expense));
            cut.Find(".delete-icon").Click();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            // Assert
            mockExpenseService.Verify(x => x.DeleteExpenseAsync(1), Times.Once());
        }

    }
}
