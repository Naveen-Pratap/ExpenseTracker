using ExpenseTracker.Controllers;
using ExpenseTracker.Repositories;
using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Controllers
{
    public class ExpenseControllerTests
    {
        [Fact]
        public async Task GetExpenses_ReturnsListOfExpenses_SortedDesc()
        {
            // Arrange
            var mockExpenseReposistory = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            var expenses = new List<Expense>
            {
                new Expense {Id = 1, Description = "Test1"},
                new Expense {Id = 2, Description = "Test2"},
            };

            mockExpenseReposistory.Setup(x => x.GetAllAsync()).ReturnsAsync(expenses);
            var controller = new ExpenseController(mockExpenseReposistory.Object, mockClock.Object);

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
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            mockExpenseRepository.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(new Expense { Id = 1, Description = "Test1" });
            var controller = new ExpenseController(mockExpenseRepository.Object, mockClock.Object);

            // Act
            var result = await controller.GetExpenseById(1);

            // Assert
            Assert.IsType<Expense>(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async Task AddExpense_AddCalled_ReturnsExpense()
        {
            // Arrange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            Expense expense = new Expense { Id = 1, Description = "dummy" };
            mockExpenseRepository.Setup(x => x.AddExpenseAsync(expense)).ReturnsAsync(expense);
            var controller = new ExpenseController(mockExpenseRepository.Object, mockClock.Object);

            // Act
            var result = await controller.AddExpense(expense);

            // Verify
            mockExpenseRepository.Verify(x => x.AddExpenseAsync(expense), Times.Once());
            Assert.IsType<Expense>(result.Value);

        }

        [Fact]
        public async Task AddExpense_SetDatetimeCorrectly()
        {
            // Arrange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            Expense expense = new Expense { Id = 1, Description = "dummy" };
            DateTime testDate = new DateTime(2024, 09, 3, 9, 6, 13);
            mockExpenseRepository.Setup(x => x.AddExpenseAsync(expense)).ReturnsAsync(expense);
            mockClock.Setup(x => x.GetLocalTimeNow()).Returns(testDate);
            var controller = new ExpenseController(mockExpenseRepository.Object, mockClock.Object);

            // Act
            var result = await controller.AddExpense(expense);

            // Verify
            Assert.IsType<DateTime>(result.Value.CreatedTime);
            Assert.Equal(testDate, result.Value.CreatedTime);
        }

        [Fact]
        public async Task UpdateExpense_BadRequestIfIdDoesntMatch()
        {
            // Arrange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            Expense expense = new Expense { Id = 1, Description = "dummy" };
            var controller = new ExpenseController(mockExpenseRepository.Object, mockClock.Object);

            // Act
            var result = await controller.UpdateExpense(2, expense);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task UpdateExpense_BadRequestIfIdDoesntExist()
        {
            // Arrange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            var controller = new ExpenseController(mockExpenseRepository.Object, mockClock.Object);
            mockExpenseRepository.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync((Expense)null);

            // Act
            var result = await controller.UpdateExpense(1, new Expense());

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);

        }

        [Fact]
        public async Task UpdateExpense_UpdateCalledAndReturnsExpense()
        {
            // Arrange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            Expense expense = new Expense { Id = 1, Description = "dummy" };
            var controller = new ExpenseController(mockExpenseRepository.Object, mockClock.Object);
            mockExpenseRepository.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(new Expense());
            mockExpenseRepository.Setup(x => x.UpdateExpenseAsync(expense)).ReturnsAsync(expense);

            // Act
            var result = await controller.UpdateExpense(1, expense);

            // Assert
            mockExpenseRepository.Verify(x => x.UpdateExpenseAsync(expense), Times.Once());
            Assert.IsType<Expense>(result.Value);
            Assert.Equal("dummy", result.Value.Description);

        }

        [Fact]
        public async Task DeleteExpense_BadRequestWhenIdDoesntExist()
        {
            // Arrange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            var controller = new ExpenseController(mockExpenseRepository.Object, mockClock.Object);
            mockExpenseRepository.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync((Expense)null);

            // Act
            var result = await controller.DeleteExpense(1);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task DeleteExpense_CallsDeleteReturnsExpense()
        {
            // Arrange
            var mockExpenseRepository = new Mock<IExpenseRepository>();
            var mockClock = new Mock<IClock>();
            Expense expense = new Expense { Id = 1, Description = "dummy" };
            var controller = new ExpenseController(mockExpenseRepository.Object, mockClock.Object);
            mockExpenseRepository.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(expense);
            mockExpenseRepository.Setup(x => x.DeleteExpenseAsync(expense)).ReturnsAsync(expense);

            // Act
            var result = await controller.DeleteExpense(1);

            // Assert
            mockExpenseRepository.Verify(x => x.DeleteExpenseAsync(expense), Times.Once());
            Assert.IsType<Expense>(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

    }
}
