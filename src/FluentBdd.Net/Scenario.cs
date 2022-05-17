using Stubble.Core.Builders;
using Stubble.Core.Settings;

namespace FluentBdd.Net
{
    public class Scenario<T>
    {
        public Feature Feature { get; }
        public string ScenarioName { get; }
        public string? ScenarioDescription { get; }
        public List<ScenarioStep<T>> Steps { get; }

        public Scenario(Feature feature, string scenarioName, string? scenarioDescription = null)
        {
            Feature = feature;
            ScenarioName = scenarioName;
            ScenarioDescription = scenarioDescription;
            Steps = new List<ScenarioStep<T>>();
        }

        public Scenario<T> Given(string description, Action<T> action)
        {
            return Insert("GIVEN", description, action);
        }

        public Scenario<T> Given(string description, Func<T, Task> action)
        {
            return Insert("GIVEN", description, action);
        }

        public Scenario<T> And(string description, Action<T> action)
        {
            return Insert("AND", description, action);
        }

        public Scenario<T> And(string description, Func<T, Task> action)
        {
            return Insert("AND", description, action);
        }

        public Scenario<T> When(string description, Action<T> action)
        {
            return Insert("WHEN", description, action);
        }

        public Scenario<T> When(string description, Func<T, Task> action)
        {
            return Insert("WHEN", description, action);
        }

        public Scenario<T> Then(string description, Action<T> action)
        {
            return Insert("THEN", description, action);
        }

        public Scenario<T> Then(string description, Func<T, Task> action)
        {
            return Insert("THEN", description, action);
        }

        public ScenarioResult<T> Execute(T context)
        {
            var scenarioResult = new ScenarioResult<T>(this, context);

            var hasAsync = Steps.Any(a => a.AsyncFunc != null);

            if (hasAsync)
            {
                throw new ArgumentException(
                    "Cannot call Execute() when there are async functions. Use ExecuteAsync() instead");
            }

            foreach (var step in Steps)
            {
                try
                {
                    step.Action?.Invoke(context);

                    var stubble = new StubbleBuilder().Build();

                    var stepDescriptionWithContextValues = stubble.Render(step.StepDescription, context, new RenderSettings
                    {
                        SkipHtmlEncoding = true
                    });

                    scenarioResult.AddStepResult(new ScenarioStepResult
                    {
                        Passed = true,
                        StepDescription = stepDescriptionWithContextValues,
                        StepType = step.StepType
                    });
                }
                catch (Exception e)
                {
                    scenarioResult.AddStepResult(new ScenarioStepResult
                    {
                        Passed = false,
                        StepDescription = step.StepDescription,
                        StepType = step.StepType,
                        ErrorMessage = $"{e.Message}\n{e.StackTrace}"
                    });
                    Feature.AddScenarioResult(scenarioResult);
                    throw;
                }
            }

            Feature.AddScenarioResult(scenarioResult);
            return scenarioResult;
        }

        public async Task<ScenarioResult<T>> ExecuteAsync(T context)
        {
            var scenarioResult = new ScenarioResult<T>(this, context);

            foreach (var step in Steps)
            {
                try
                {
                    step.Action?.Invoke(context);

                    if (step.AsyncFunc != null)
                    {
                        await step.AsyncFunc(context);
                    }

                    var stubble = new StubbleBuilder().Build();

                    var stepDescriptionWithContextValues = await stubble.RenderAsync(step.StepDescription, context, new RenderSettings
                    {
                        SkipHtmlEncoding = true
                    });

                    scenarioResult.AddStepResult(new ScenarioStepResult
                    {
                        Passed = true,
                        StepDescription = stepDescriptionWithContextValues,
                        StepType = step.StepType
                    });
                }
                catch (Exception e)
                {
                    scenarioResult.AddStepResult(new ScenarioStepResult
                    {
                        Passed = false,
                        StepDescription = step.StepDescription,
                        StepType = step.StepType,
                        ErrorMessage = $"{e.Message}\n{e.StackTrace}"
                    });
                    Feature.AddScenarioResult(scenarioResult);
                    throw;
                }
            }

            Feature.AddScenarioResult(scenarioResult);
            return scenarioResult;
        }

        private Scenario<T> Insert(string lineType, string description, Action<T> action)
        {
            Steps.Add(new ScenarioStep<T>
            {
                Action = action,
                StepDescription = description,
                StepType = lineType
            });

            return this;
        }

        private Scenario<T> Insert(string lineType, string description, Func<T, Task> action)
        {
            Steps.Add(new ScenarioStep<T>
            {
                AsyncFunc = action,
                StepDescription = description,
                StepType = lineType
            });

            return this;
        }
    }
}
