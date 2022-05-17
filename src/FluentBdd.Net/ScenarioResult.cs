namespace FluentBdd.Net;

public class ScenarioResult<T> : IScenarioResult
{
    private readonly Scenario<T> _scenario;
    private readonly T _context;
    public object? Context => _context;
    public string ScenarioName => _scenario.ScenarioName;
    public string? ScenarioDescription => _scenario.ScenarioDescription;
    public List<ScenarioStepResult> ScenarioStepResults { get; }

    public ScenarioResult(Scenario<T> scenario, T context)
    {
        _scenario = scenario;
        _context = context;
        ScenarioStepResults = new List<ScenarioStepResult>();
    }

    public void AddStepResult(ScenarioStepResult result)
    {
        ScenarioStepResults.Add(result);
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
