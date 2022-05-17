namespace FluentBdd.Net;

public interface IScenarioResult
{
    string ScenarioName { get; }
    List<ScenarioStepResult> ScenarioStepResults { get; }
    void AddStepResult(ScenarioStepResult result);
    object? Context { get; }
    //string Content { get; }
}
