using ExpenseTracker.Controllers;
using ExpenseTracker.Repositories;
using ExpenseTracker.Services;
using ExpenseTracker.Utils;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Services
{
    public class ExpenseAnalyticsServiceTests
    {
        [Fact]
        public async Task GetTotalExpenseByTagAsync_ReturnsCorrectValue()
        {
            // Arrange
            var mockExpenseReposistory = new Mock<IExpenseRepository>();
            var tag1 = new ExpenseTag
            {
                Id = 1,
                Name = "Test1",
            };
            var tag2 = new ExpenseTag
            {
                Id = 2,
                Name = "Test2",
            };
            var expenses = new List<Expense>
            {
                new Expense {Id = 1, Description = "Test1", Tag = tag1, Tagid = tag1.Id, Value = 100, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 13)},
                new Expense {Id = 2, Description = "Test2", Tag = tag2, Tagid = tag2.Id, Value = 150, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 14)},
                new Expense {Id = 3, Description = "Test1", Tag = tag1, Tagid = tag1.Id, Value = 50, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 14)},
                new Expense {Id = 4, Description = "Test2", Tag = tag2, Tagid = tag2.Id, Value = 10, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 13)},
                new Expense {Id = 5, Description = "Test2", Tag = tag2, Tagid = tag2.Id, Value = 10, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 12)},
            };

            mockExpenseReposistory.Setup(x => x.GetAllAsync()).ReturnsAsync(expenses);
            var service = new ExpenseAnalyticsService(mockExpenseReposistory.Object);

            // Act
            var result = await service.GetTotalExpenseByTagAsync(new DateTime(2024, 09, 3, 9, 6, 13));

            // Assert
            Assert.IsType<Dictionary<string, int>>(result);
            Assert.Equal(150, result[tag1.Name]);
            Assert.Equal(160, result[tag2.Name]);
        }

        [Fact]
        public async Task GetTotalExpense_ReturnsCorrectValue()
        {
            // Arrange
            var mockExpenseReposistory = new Mock<IExpenseRepository>();

            var expenses = new List<Expense>
            {
                new Expense {Id = 1, Description = "Test1", Value = 100, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 13)},
                new Expense {Id = 2, Description = "Test2", Value = 150, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 14)},
                new Expense {Id = 3, Description = "Test1", Value = 50, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 14)},
                new Expense {Id = 4, Description = "Test2", Value = 10, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 13)},
                new Expense {Id = 5, Description = "Test2", Value = 10, CreatedTime = new DateTime(2024, 09, 3, 9, 6, 12)},
            };

            mockExpenseReposistory.Setup(x => x.GetAllAsync()).ReturnsAsync(expenses);
            var service = new ExpenseAnalyticsService(mockExpenseReposistory.Object);

            // Act
            var result = await service.GetTotalExpenseAsync(new DateTime(2024, 09, 3, 9, 6, 13));

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(310, result);
        }

    }
}
