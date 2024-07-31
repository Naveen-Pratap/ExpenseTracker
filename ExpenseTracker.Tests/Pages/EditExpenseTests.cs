using ExpenseTracker.Components.Pages;
using ExpenseTracker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Pages
{
    public class EditExpenseTests: TestContext
    {
        [Fact]
        public void EditExpense_HasHeading()
        {
            // Arrange
            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(new Expense { Id = 1 });

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(new List<ExpenseTag>());
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));

            var headingElem = cut.Find(".heading");

            // Assert
            Assert.NotNull(headingElem);

        }

        [Fact]
        public void EditExpense_ExpenseLoadedCorrectlyOnParameterSet()
        {
            // Arrange
            Expense expense = new Expense { Id=1};
            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(expense);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(new List<ExpenseTag>());
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));


            // Assert
            Assert.Equal(1, cut.Instance.Expense.Id);

        }

        [Fact]
        public void EditExpense_TagsInFormRenderedCorrectly()
        {
            // Arrange
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
                new ExpenseTag {Id = 2, Name= "dummy2"},
            };
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(new Expense { Id = 1 });
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));

            var tagElem = cut.Find("#tag");

            // Assert
            // +1 for default option - <option value="""">Select Tag</option>
            Assert.Equal(tagElem.ChildNodes.Length, expenseTags.Count() + 1);

            for (var i = 0; i < expenseTags.Count(); i++)
            {
                Assert.Equal(tagElem.ChildNodes[i + 1].TextContent, expenseTags[i].Name);
            }

        }

        [Fact]
        public void EditExpense_FormSubmittedCorrectly()
        {
            // Arrange
            Expense expense = new Expense { Id = 1 };
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
            };
            var newExpense = new Expense();
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(expense);
            mockExpenseService.Setup(x => x.EditExpenseAsync(1, expense));
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);


            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));

            var inputElemValue = cut.Find("#value");
            inputElemValue.Change("200");

            var inputElemDescription = cut.Find("#description");
            inputElemDescription.Change("Dummy description");

            var inputElemTag = cut.Find("#tag");
            inputElemTag.Change(1);

            cut.Find(".btn").Click();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            // Assert
            Assert.Equal(200, cut.Instance.Expense.Value);
            Assert.Equal("Dummy description", cut.Instance.Expense.Description);
            Assert.Equal(1, cut.Instance.Expense.Tagid);
            mockExpenseService.Verify(x => x.EditExpenseAsync(1, It.IsAny<Expense>()), Times.Once());
            Assert.Equal($"http://localhost/", navMan.Uri);

        }

        [Fact]
        public void EditExpense_ShowsErrorMessageWhenTagsServiceRaiseHttpRequestException()
        {
            // Arrange
            Expense expense = new Expense { Id = 1 };
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
            };
            var newExpense = new Expense();
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(expense);
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.Setup(x => x.LoadExpenseTagsAsync()).Throws<HttpRequestException>();
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));

            var errorElem = cut.Find(".error-message-simple");

            // Assert
            Assert.NotNull(errorElem);
            Assert.True(cut.Instance.Error);
            Assert.NotEmpty(cut.Instance.ErrorMessage);
        }

        [Fact]
        public void EditExpense_ShowsErrorMessageWhenTagsServiceRaisetException()
        {
            // Arrange
            Expense expense = new Expense { Id = 1 };
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
            };
            var newExpense = new Expense();
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(expense);
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.Setup(x => x.LoadExpenseTagsAsync()).Throws<Exception>();
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));

            var errorElem = cut.Find(".error-message-simple");

            // Assert
            Assert.NotNull(errorElem);
            Assert.True(cut.Instance.Error);
            Assert.NotEmpty(cut.Instance.ErrorMessage);
        }

        [Fact]
        public void EditExpense_ShowsErrorMessageWhenGetExpenseByIdAsyncRaiseHttpRequestException()
        {
            // Arrange
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).Throws<HttpRequestException>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(new List<ExpenseTag>());
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));

            var errorElem = cut.Find(".error-message-simple");

            // Assert
            Assert.NotNull(errorElem);
            Assert.True(cut.Instance.Error);
            Assert.NotEmpty(cut.Instance.ErrorMessage);
        }

        [Fact]
        public void EditExpense_ShowsErrorMessageWhenGetExpenseByIdAsyncRaiseException()
        {
            // Arrange
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).Throws<Exception>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(new List<ExpenseTag>());
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));

            var errorElem = cut.Find(".error-message-simple");

            // Assert
            Assert.NotNull(errorElem);
            Assert.True(cut.Instance.Error);
            Assert.NotEmpty(cut.Instance.ErrorMessage);
        }

        [Fact]
        public void EditExpense_ShowsErrorMessageWheEditExpenseAsyncRaiseHttpRequestException()
        {
            // Arrange
            Expense expense = new Expense { Id = 1 };
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
            };
            var newExpense = new Expense();
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(expense);
            mockExpenseService.Setup(x => x.EditExpenseAsync(1, It.IsAny<Expense>())).Throws<HttpRequestException>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));
            var inputElemValue = cut.Find("#value");
            inputElemValue.Change("200");

            var inputElemDescription = cut.Find("#description");
            inputElemDescription.Change("Dummy description");

            var inputElemTag = cut.Find("#tag");
            inputElemTag.Change(1);
            cut.Find(".btn").Click();

            var errorElem = cut.Find(".error-message-simple");

            // Assert
            Assert.NotNull(errorElem);
            Assert.True(cut.Instance.Error);
            Assert.NotEmpty(cut.Instance.ErrorMessage);
        }  
        
        [Fact]
        public void EditExpense_ShowsErrorMessageWheEditExpenseAsyncRaiseException()
        {
            // Arrange
            Expense expense = new Expense { Id = 1 };
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
            };
            var newExpense = new Expense();
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.GetExpenseByIdAsync(1)).ReturnsAsync(expense);
            mockExpenseService.Setup(x => x.EditExpenseAsync(1, It.IsAny<Expense>())).Throws<Exception>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<EditExpense>(parameters => parameters.Add(p => p.Id, 1));
            var inputElemValue = cut.Find("#value");
            inputElemValue.Change("200");

            var inputElemDescription = cut.Find("#description");
            inputElemDescription.Change("Dummy description");

            var inputElemTag = cut.Find("#tag");
            inputElemTag.Change(1);
            cut.Find(".btn").Click();

            var errorElem = cut.Find(".error-message-simple");

            // Assert
            Assert.NotNull(errorElem);
            Assert.True(cut.Instance.Error);
            Assert.NotEmpty(cut.Instance.ErrorMessage);
        }
    }
}
