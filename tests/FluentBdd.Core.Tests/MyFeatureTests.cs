using System.Threading.Tasks;
using RestSharp;
using Xunit;

namespace FluentBdd.Core.Tests
{
    public class MyFeatureTests : IClassFixture<MyFeatureFixture>
    {
        public MyFeatureFixture MyFeatureFixture { get; }

        public MyFeatureTests(MyFeatureFixture myFeatureFixture)
        {
            MyFeatureFixture = myFeatureFixture;
        }

        public class MyCustomContext
        {
            public string? FirstName { get; set; }
            public int? Age { get; set; }
            public RestResponse<AgeResult>? Result { get; set; }
        }

        public class AgeResult
        {
            public string? Name { get; set; }
            public int Age { get; set; }
        }

        [Theory]
        [InlineData("bob", 53)]
        [InlineData("taylor", 34)]
        public async Task ShouldPredictAge(string expectedFirstName, int expectedAge)
        {
            var scenario = new Scenario<MyCustomContext>(
                    MyFeatureFixture.Feature,
                    "My Scenario",
                    scenarioDescription: "Lorem ipsum dolor sit amet.")
                .Given("my first name is {{FirstName}}", (context) =>
                {
                    context.FirstName = expectedFirstName;
                    context.Age = expectedAge;
                })
                .When("I predict my age", async (context) =>
                {
                    var client = new RestClient("https://api.agify.io");

                    var restRequest = new RestRequest($"?name={context.FirstName}");
                    var result = await client.ExecuteAsync<AgeResult>(restRequest);

                    context.Result = result;
                })
                .Then("I should see {{Age}} as my age", (context) =>
                {
                    Assert.True(context?.Result?.IsSuccessful);
                    Assert.Equal(expectedFirstName, context?.Result?.Data?.Name);
                    Assert.Equal(expectedAge, context?.Result?.Data?.Age);
                });

            await scenario.ExecuteAsync(new MyCustomContext
            {
                FirstName = expectedFirstName
            });
        }
    }
}
