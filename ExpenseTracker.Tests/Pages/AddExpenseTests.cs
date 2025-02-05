﻿using ExpenseTracker.Components.Pages;
using ExpenseTracker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExpenseTracker.Tests.Pages
{
    public class AddExpenseTests : TestContext
    {

        [Fact]
        public void AddExpense_TagsInFormRenderedCorrectly()
        {
            // Arrange
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
                new ExpenseTag {Id = 2, Name= "dummy2"},
            };
            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<AddExpense>();

            var tagElem = cut.Find("#tag");

            // +1 for default option - <option value="""">Select Tag</option>
            Assert.Equal(tagElem.ChildNodes.Length, expenseTags.Count() + 1);

            for (var i = 0; i < expenseTags.Count(); i++)
            {
                Assert.Equal(tagElem.ChildNodes[i + 1].TextContent, expenseTags[i].Name);
            }

        }

        [Fact]
        public void AddExpense_FormbindedCorrectly()
        {
            // Arrange
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1}
            };
            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);


            // Act
            var cut = RenderComponent<AddExpense>();

            var inputElemValue = cut.Find("#value");
            inputElemValue.Change("200");

            var inputElemDescription = cut.Find("#description");
            inputElemDescription.Change("Dummy description");

            var inputElemTag = cut.Find("#tag");
            inputElemTag.Change(1);

            // Assert
            Assert.Equal(200, cut!.Instance!.NewExpense!.Value);
            Assert.Equal("Dummy description", cut!.Instance!.NewExpense!.Description);
            Assert.Equal(1, cut!.Instance!.NewExpense!.Tagid);


        }

        [Fact]
        public void AddExpense_FormSubmittedCorrectly()
        {
            // Arrange
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
            };
            var newExpense = new Expense();
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.AddExpenseAsync(newExpense));
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);


            // Act
            var cut = RenderComponent<AddExpense>();

            var inputElemValue = cut.Find("#value");
            inputElemValue.Change("200");

            var inputElemDescription = cut.Find("#description");
            inputElemDescription.Change("Dummy description");

            var inputElemTag = cut.Find("#tag");
            inputElemTag.Change(1);

            cut.Find(".btn").Click();

            var navMan = Services.GetRequiredService<FakeNavigationManager>();

            // Assert
            mockExpenseService.Verify(x => x.AddExpenseAsync(It.IsAny<Expense>()), Times.Once());
            Assert.Equal($"http://localhost/", navMan.Uri);

        }

        [Fact]
        public void AddExpense_HasHeading()
        {
            // Arrange
            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(new List<ExpenseTag>());
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<AddExpense>();

            var headingElem = cut.Find(".heading");

            // Assert
            Assert.NotNull(headingElem);

        }

        [Fact]
        public void AddExpense_ShowsErrorMessageWhenTagsServiceRaiseHttpRequestException()
        {
            // Arrange
            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.Setup(x => x.LoadExpenseTagsAsync()).Throws<HttpRequestException>();
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<AddExpense>();

            var errorElem = cut.Find(".error-message-simple");

            // Assert
            Assert.NotNull(errorElem);
            Assert.True(cut.Instance.Error);
            Assert.NotEmpty(cut.Instance.ErrorMessage);
        }

        [Fact]
        public void AddExpense_ShowsErrorMessageWhenTagsServiceRaisetException()
        {
            // Arrange
            var mockExpenseService = new Mock<IExpenseService>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.Setup(x => x.LoadExpenseTagsAsync()).Throws<Exception>();
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<AddExpense>();

            var errorElem = cut.Find(".error-message-simple");

            // Assert
            Assert.NotNull(errorElem);
            Assert.True(cut.Instance.Error);
            Assert.NotEmpty(cut.Instance.ErrorMessage);
        }

        [Fact]
        public void AddExpense_ShowsErrorMessageWhenExpenseServiceRaiseHttpRequestException()
        {
            // Arrange
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
            };
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.AddExpenseAsync(It.IsAny<Expense>())).Throws<HttpRequestException>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<AddExpense>();
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
        public void AddExpense_ShowsErrorMessageWhenExpenseServiceRaiseException()
        {
            // Arrange
            var expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag {Id = 1, Name= "dummy1"},
            };
            var mockExpenseService = new Mock<IExpenseService>();
            mockExpenseService.Setup(x => x.AddExpenseAsync(It.IsAny<Expense>())).Throws<Exception>();
            Services.AddSingleton<IExpenseService>(mockExpenseService.Object);

            var mockExpenseTagService = new Mock<IExpenseTagService>();
            mockExpenseTagService.SetupGet(x => x.ExpenseTags).Returns(expenseTags);
            Services.AddSingleton<IExpenseTagService>(mockExpenseTagService.Object);

            // Act
            var cut = RenderComponent<AddExpense>();
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
