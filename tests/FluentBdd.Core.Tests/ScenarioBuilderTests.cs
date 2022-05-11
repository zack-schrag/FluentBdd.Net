using System;
using System.Threading.Tasks;
using RestSharp;
using Xunit;

namespace FluentBdd.Core.Tests
{
    public class ScenarioBuilderTests : IClassFixture<MyFeatureFixture>
    {
        public MyFeatureFixture MyFeatureFixture { get; }

        public ScenarioBuilderTests(MyFeatureFixture myFeatureFixture)
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
        [InlineData("taylor", 53)]
        public async Task ShouldGenerateScenarioText(string expectedFirstName, int expectedAge)
        {
            var scenario = new Scenario<MyCustomContext>(
                    MyFeatureFixture.Feature,
                    "My Scenario",
                    scenarioDescription: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ornare quam viverra orci sagittis eu volutpat. Tortor id aliquet lectus proin nibh nisl. Proin sagittis nisl rhoncus mattis rhoncus urna neque.")
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
                .Then("I should see {{Age}} as my age", async (context) =>
                {
                    await Assert.ThrowsAsync<ArgumentException>(() => throw new ArgumentException("blah"));
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
