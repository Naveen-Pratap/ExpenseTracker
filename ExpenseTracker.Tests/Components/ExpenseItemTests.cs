using System;
using ExpenseTracker.Components;

namespace ExpenseTracker.Tests.Components
{

    public class ExpenseItemTests : TestContext
    {
        [Fact]
        public void ExpenseItem_DisplaysCorrectInfo()
        {
            var tag = new ExpenseTag
            {
                Id = 1,
                Name = "dummy"
            };

            var expense = new Expense
            {
                Id = 1,
                Value = 100,
                Description = "Test",
                CreatedTime = DateTime.Now,
                Tag = tag,
            };

            var cut = RenderComponent<ExpenseItem>(parameters => parameters.Add(p => p.expense, expense));
            var valueElem = cut.Find(".value");
            var descriptionElem = cut.Find(".description");
            var createdTimeElem = cut.Find(".created-time");

            Assert.Equal(expense.Value.ToString(), valueElem.TextContent.Trim());
            Assert.Equal(expense.Description.ToString(), descriptionElem.TextContent.Trim());
            Assert.Equal(expense.CreatedTime.ToString(), createdTimeElem.TextContent.Trim());
        }

    }
}
