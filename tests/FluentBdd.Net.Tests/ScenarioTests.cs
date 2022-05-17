using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentBdd.Net.Tests
{
    public class ScenarioTests
    {
        [Fact]
        public void Given_ShouldAddStep()
        {
            var scenario = new Scenario<object>(null!, "name")
                .Given("Foo bar", (ctx) => Console.WriteLine("foo bar"));

            Assert.Equal(1, scenario?.Steps.Count);
            Assert.Equal("Foo bar", scenario?.Steps[0].StepDescription);
            Assert.Equal("GIVEN", scenario?.Steps[0].StepType);
            Assert.NotNull(scenario?.Steps[0].Action);
            Assert.Null(scenario?.Steps[0].AsyncFunc);
        }

        [Fact]
        public void Given_ShouldAddAsyncStep()
        {
            var scenario = new Scenario<object>(null!, "name")
                .Given("Foo bar", async (ctx) => await Console.Out.WriteLineAsync("foo bar"));

            Assert.Equal(1, scenario?.Steps.Count);
            Assert.Equal("Foo bar", scenario?.Steps[0].StepDescription);
            Assert.Equal("GIVEN", scenario?.Steps[0].StepType);
            Assert.Null(scenario?.Steps[0].Action);
            Assert.NotNull(scenario?.Steps[0].AsyncFunc);
        }

        [Fact]
        public void And_ShouldAddStep()
        {
            var scenario = new Scenario<object>(null!, "name")
                .And("Foo bar", (ctx) => Console.WriteLine("foo bar"));

            Assert.Equal(1, scenario?.Steps.Count);
            Assert.Equal("Foo bar", scenario?.Steps[0].StepDescription);
            Assert.Equal("AND", scenario?.Steps[0].StepType);
            Assert.NotNull(scenario?.Steps[0].Action);
            Assert.Null(scenario?.Steps[0].AsyncFunc);
        }

        [Fact]
        public void And_ShouldAddAsyncStep()
        {
            var scenario = new Scenario<object>(null!, "name")
                .And("Foo bar", async (ctx) => await Console.Out.WriteLineAsync("foo bar"));

            Assert.Equal(1, scenario?.Steps.Count);
            Assert.Equal("Foo bar", scenario?.Steps[0].StepDescription);
            Assert.Equal("AND", scenario?.Steps[0].StepType);
            Assert.Null(scenario?.Steps[0].Action);
            Assert.NotNull(scenario?.Steps[0].AsyncFunc);
        }

        [Fact]
        public void When_ShouldAddStep()
        {
            var scenario = new Scenario<object>(null!, "name")
                .When("Foo bar", (ctx) => Console.WriteLine("foo bar"));

            Assert.Equal(1, scenario?.Steps.Count);
            Assert.Equal("Foo bar", scenario?.Steps[0].StepDescription);
            Assert.Equal("WHEN", scenario?.Steps[0].StepType);
            Assert.NotNull(scenario?.Steps[0].Action);
            Assert.Null(scenario?.Steps[0].AsyncFunc);
        }

        [Fact]
        public void When_ShouldAddAsyncStep()
        {
            var scenario = new Scenario<object>(null!, "name")
                .When("Foo bar", async (ctx) => await Console.Out.WriteLineAsync("foo bar"));

            Assert.Equal(1, scenario?.Steps.Count);
            Assert.Equal("Foo bar", scenario?.Steps[0].StepDescription);
            Assert.Equal("WHEN", scenario?.Steps[0].StepType);
            Assert.Null(scenario?.Steps[0].Action);
            Assert.NotNull(scenario?.Steps[0].AsyncFunc);
        }

        [Fact]
        public void Then_ShouldAddStep()
        {
            var scenario = new Scenario<object>(null!, "name")
                .Then("Foo bar", (ctx) => Console.WriteLine("foo bar"));

            Assert.Equal(1, scenario?.Steps.Count);
            Assert.Equal("Foo bar", scenario?.Steps[0].StepDescription);
            Assert.Equal("THEN", scenario?.Steps[0].StepType);
            Assert.NotNull(scenario?.Steps[0].Action);
            Assert.Null(scenario?.Steps[0].AsyncFunc);
        }

        [Fact]
        public void Then_ShouldAddAsyncStep()
        {
            var scenario = new Scenario<object>(null!, "name")
                .Then("Foo bar", async (ctx) => await Console.Out.WriteLineAsync("foo bar"));

            Assert.Equal(1, scenario?.Steps.Count);
            Assert.Equal("Foo bar", scenario?.Steps[0].StepDescription);
            Assert.Equal("THEN", scenario?.Steps[0].StepType);
            Assert.Null(scenario?.Steps[0].Action);
            Assert.NotNull(scenario?.Steps[0].AsyncFunc);
        }

        [Fact]
        public void Execute_ShouldThrowException_WhenThereAreAsyncFunctions()
        {
            var scenario = new Scenario<object>(null!, "name")
                .Given("Foo bar", async (ctx) => await Console.Out.WriteLineAsync("foo bar"));

            Assert.Throws<ArgumentException>(() => scenario.Execute(null!));
        }

        [Fact]
        public void Execute_ShouldAddStepResult()
        {
            var feature = new Feature("My feature");

            var scenario = new Scenario<object>(feature, "scenario name")
                .Given("Foo bar {{Baz}}", null!);

            var context = new { Baz = "123" };
            var result = scenario.Execute(context);

            Assert.Equal("Foo bar 123", result.ScenarioStepResults[0].StepDescription);
            Assert.True(result.ScenarioStepResults[0].Passed);
            Assert.Null(result.ScenarioStepResults[0].ErrorMessage);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldAddStepResult()
        {
            var feature = new Feature("My feature");

            var scenario = new Scenario<object>(feature, "scenario name")
                .Given("Foo bar {{Baz}}", null!);

            var context = new { Baz = "123" };
            var result = await scenario.ExecuteAsync(context);

            Assert.Equal("Foo bar 123", result.ScenarioStepResults[0].StepDescription);
            Assert.True(result.ScenarioStepResults[0].Passed);
            Assert.Null(result.ScenarioStepResults[0].ErrorMessage);
        }

        [Fact]
        public void Execute_ShouldAddError_WhenExceptionIsThrown()
        {
            var feature = new Feature("My feature");

            var scenario = new Scenario<object>(feature, "scenario name")
                .Given("Foo bar {{Baz}}", (_) => Assert.False(true));

            var context = new { Baz = "123" };
            
            Assert.Throws<FalseException>(() => scenario.Execute(context));

            Assert.False(feature.ScenarioResults[0].ScenarioStepResults[0].Passed);
            Assert.NotNull(feature.ScenarioResults[0].ScenarioStepResults[0].ErrorMessage);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldAddError_WhenExceptionIsThrown()
        {
            var feature = new Feature("My feature");

            var scenario = new Scenario<object>(feature, "scenario name")
                .Given("Foo bar {{Baz}}", (_) => Assert.False(true));

            var context = new { Baz = "123" };

            await Assert.ThrowsAsync<FalseException>(async () =>
            {
                await scenario.ExecuteAsync(context);
            });

            Assert.False(feature.ScenarioResults[0].ScenarioStepResults[0].Passed);
            Assert.NotNull(feature.ScenarioResults[0].ScenarioStepResults[0].ErrorMessage);
        }
    }
}
