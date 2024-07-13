using System;
using System.Collections.Generic;
using ExpenseTracker.Components;

namespace ExpenseTracker.Tests.Components
{

    public class ExpenseListsTests : TestContext
    {
        [Fact]
        public void ExpenseList_DisplaysCorrectInfo()
        {
            var tag = new ExpenseTag
            {
                Id = 1,
                Name = "dummy"
            };

            var expenses = new List<Expense>
            {
                new Expense { Id = 1, Value = 100, Description = "Expense 1", Tag = tag, CreatedTime = DateTime.Now },
                new Expense { Id = 2, Value = 200, Description = "Expense 2", Tag = tag, CreatedTime = DateTime.Now }
            };

            var cut = RenderComponent<ExpenseList>(parameters => parameters.Add(p => p.expenses, expenses));

            for (var i = 0; i < expenses.Count; i++)
            {
                var valueElem = cut.FindAll(".value")[i];
                var descriptionElem = cut.FindAll(".description")[i];
                var createdTimeElem = cut.FindAll(".created-time")[i];

                Assert.Equal(expenses[i].Value.ToString(), valueElem.TextContent.Trim());
                Assert.Equal(expenses[i].Description.ToString(), descriptionElem.TextContent.Trim());
                Assert.Equal(expenses[i].CreatedTime.ToString(), createdTimeElem.TextContent.Trim());
            }

        }

    }
}
