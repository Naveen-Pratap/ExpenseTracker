using System;
using System.Collections.Generic;
using System.Net.Http;
using ExpenseTracker.Components;
using ExpenseTracker.Services;
using RichardSzalay.MockHttp;
using Bunit;

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

            // Assert
            var headingElem = cut.Find(".heading");
            var cardsElem = cut.Find(".expense-cards");
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

            // Assert
            var cardsElem = cut.Find(".expense-cards");
            Assert.Equal(2, cardsElem.ChildElementCount);
        }

    }
}
