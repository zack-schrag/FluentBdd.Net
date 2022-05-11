namespace FluentBdd.Core;

public class ScenarioStep<T>
{
    public string? StepType { get; set; }
    public string? StepDescription { get; set; }
    public Action<T>? Action { get; set; }
    public Func<T, Task>? AsyncFunc { get; set; }
}
