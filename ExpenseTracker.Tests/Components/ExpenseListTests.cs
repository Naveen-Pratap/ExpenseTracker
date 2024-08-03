using ExpenseTracker.Components;
using System.Collections.Generic;


namespace ExpenseTracker.Tests.Components
{

    public class ExpenseListsTests : TestContext
    {
        [Fact]
        public void ExpenseList_CallsChildComponentForEachExpense()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense(),
                new Expense()
            };

            ComponentFactories.AddStub<ExpenseItem>();

            // Act
            var cut = RenderComponent<ExpenseList>(parameters => parameters.Add(p => p.expenses, expenses));

            // Assert
            Assert.Equal(2, cut.FindComponents<Stub<ExpenseItem>>().Count);
        }

        [Fact]
        public void ExpenseList_HasHeadingAndExpenseCards()
        {
            // Arrange
            ComponentFactories.AddStub<ExpenseItem>();

            // Act
            var cut = RenderComponent<ExpenseList>(parameters => parameters.Add(p => p.expenses, new List<Expense>()));


            var headingElem = cut.Find(".heading");
            var cardsElem = cut.Find(".expense-cards");

            // Assert
            Assert.NotNull(headingElem);
            Assert.NotNull(cardsElem);

        }

        [Fact]
        public void ExpenseList_ExpenseCardsLoadsWithEachExpense()
        {
            // Arrange
            var expenses = new List<Expense>
            {
                new Expense(),
                new Expense()
            };

            ComponentFactories.AddStub<ExpenseItem>();

            // Act
            var cut = RenderComponent<ExpenseList>(parameters => parameters.Add(p => p.expenses, expenses));

            var cardsElem = cut.Find(".expense-cards");

            // Assert
            Assert.Equal(2, cardsElem.ChildElementCount);
        }

    }
}
