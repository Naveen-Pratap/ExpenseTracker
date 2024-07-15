using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Components.Pages;
using Microsoft.AspNetCore.Components;
using Moq;
using ExpenseTracker.Tests;

namespace ExpenseTracker.Tests.Pages
{
    public class AddExpenseTests : TestContext
    {
        private readonly Mock<HttpClient>? _httpClientMock;
        private readonly Mock<NavigationManager>? _navigationManagerMock;
        private readonly List<ExpenseTag> expenseTags;

        public AddExpenseTests()
        {
            _navigationManagerMock = new Mock<NavigationManager>();
            Services.AddSingleton(_navigationManagerMock);

            expenseTags = new List<ExpenseTag>
            {
                new ExpenseTag
                {
                    Id = 1,
                    Name = "Test",
                },
                new ExpenseTag
                {
                    Id = 2,
                    Name = "Test2",
                }
            };

            var mockHttpClient = MockHttpClientHelper.CreateMockHttpClient(expenseTags, new Uri("http://localhost/"));
            Services.AddSingleton(mockHttpClient);
        }

        [Fact]
        public void AddExpense_TagsInFormRenderedCorrectly()
        {
            var cut = RenderComponent<AddExpense>();

            var tagElem = cut.Find("#tag");

            // +1 for default option - <option value="""">Select Tag</option>
            Assert.Equal(tagElem.ChildNodes.Length, expenseTags.Count() + 1);

            for (var i=0; i < expenseTags.Count(); i++)
            {
                Assert.Equal(tagElem.ChildNodes[i + 1].TextContent, expenseTags[i].Name);
            }

        }

        [Fact]
        public void AddExpense_FormbindedCorrectly()
        {
            var cut = RenderComponent<AddExpense>();

            var inputElemValue = cut.Find("#value");
            inputElemValue.Change("200");

            var inputElemDescription = cut.Find("#description");
            inputElemDescription.Change("Dummy description");
            
            var inputElemTag = cut.Find("#tag");
            inputElemTag.Change(1);

            Assert.Equal(200, cut!.Instance!.newExpense!.Value);
            Assert.Equal("Dummy description", cut!.Instance!.newExpense!.Description);
            Assert.Equal(1, cut!.Instance!.newExpense!.Tagid);


        }
    }
}
