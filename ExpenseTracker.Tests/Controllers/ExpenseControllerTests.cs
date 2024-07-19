using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Controllers;
using Moq;
using Moq.EntityFrameworkCore;

using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Tests.Controllers
{
    public class ExpenseControllerTests
    {
        [Fact]
        public async Task GetExpenses_ReturnsListOfExpenses_SortedDesc()
        {
            // Arrange
            var mockExpenseContext = new Mock<ExpenseContext>();
            var expenses = new List<Expense>
            {
                new Expense {Id = 1, Description = "Test1"},
                new Expense {Id = 2, Description = "Test2"},
            };

            mockExpenseContext.Setup(x => x.Expenses).ReturnsDbSet(expenses);
            var controller = new ExpenseController(mockExpenseContext.Object);

            // Act
            var result = await controller.GetExpenses();

            // Assert
            Assert.IsType<List<Expense>>(result.Value);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal(2, result.Value[0].Id);

        }

        [Fact]
        public async Task GetExpenseById_ReturnsCorrectExpense()
        {
            // Arrange
            var mockExpenseContext = new Mock<ExpenseContext>();
            var expenses = new List<Expense>
            {
                new Expense {Id = 1, Description = "Test1"},
                new Expense {Id = 2, Description = "Test2"},
            };

            mockExpenseContext.Setup(x => x.Expenses).ReturnsDbSet(expenses);
            var controller = new ExpenseController(mockExpenseContext.Object);

            // Act
            var result = await controller.GetExpenseById(1);

            // Assert
            Assert.IsType<Expense>(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        public async Task AddExpense_CreatesNewExpense() { 
        }
    }
}
