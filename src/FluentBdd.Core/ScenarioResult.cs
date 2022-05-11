using System.Text;
using Stubble.Core.Builders;

namespace FluentBdd.Core;


public interface IScenarioResult
{
    string ScenarioName { get; }
    List<ScenarioStepResult> ScenarioStepResults { get; }
    void AddStepResult(ScenarioStepResult result);
    object? Context { get; }
    string Content { get; }
}

public class ScenarioResult<T> : IScenarioResult
{
    private readonly Scenario<T> _scenario;
    private readonly T? _context;
    public object? Context => _context;
    public string ScenarioName => _scenario.ScenarioName;
    public string ScenarioDescription => _scenario.ScenarioDescription;
    public List<ScenarioStepResult> ScenarioStepResults { get; }

    public ScenarioResult(Scenario<T> scenario, T? context)
    {
        _scenario = scenario;
        _context = context;
        ScenarioStepResults = new List<ScenarioStepResult>();
    }

    public void AddStepResult(ScenarioStepResult result)
    {
        ScenarioStepResults.Add(result);
    }

    public string Content
    {
        get
        {
            var stubble = new StubbleBuilder().Build();

            using StreamReader streamReader = new("scenario_result_template.md", Encoding.UTF8);
            // first render output for scenario
            var output = stubble.Render(streamReader.ReadToEnd(), this);

            // then apply context params to generated output
            return stubble.Render(output, Context);
        }
    }
}

public class ScenarioStepResult
{
    public bool Passed { get; set; }
    public bool Failed => !Passed;
    public string? StepDescription { get; set; }
    public string? StepType { get; set; }
    public string? ErrorMessage { get; set; }
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
}
