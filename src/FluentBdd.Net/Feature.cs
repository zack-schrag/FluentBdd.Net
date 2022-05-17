namespace FluentBdd.Net;

public class Feature
{
    private readonly IResultFormatter _resultFormatter;
    public List<IScenarioResult> ScenarioResults { get; }
    public string Name { get; }
    public string? Description { get; }
    public List<string> _tags;


    public Feature(string name, string? description = null, IResultFormatter? resultFormatter = null)
    {
        Name = name;
        Description = description;
        _tags = new List<string>();
        _resultFormatter = resultFormatter ?? new MarkdownFormatter();
        ScenarioResults = new List<IScenarioResult>();
    }

    public void AddTag(string tag)
    {
        _tags.Add(tag);
    }

    public void AddScenarioResult(IScenarioResult result)
    {
        ScenarioResults.Add(result);
    }

    public void Output()
    {
        _resultFormatter.Output(this);
    }
}
